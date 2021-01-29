using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TwitterApi.Core.Contracts.Common;
using TwitterApi.Core.Contracts.User;
using TwitterApi.DataLayer.Common;
using TwitterApi.DataLayer.Entities.Models;
using TwitterApi.DataLayer.Extensions;
using TwitterApi.DataLayer.Utils;

namespace TwitterApi.Core.Controllers
{
    /// <summary>
    /// Контроллер для работы с пользователями
    /// </summary>
    [ApiController]
    [Authorize]
    [Produces("application/json")]
    public class UsersController : ControllerBase
    {
        private readonly TwitterDbContext _dbContext;

        /// <summary>
        /// Контроллер для работы с пользователями
        /// </summary>
        public UsersController(DbContextFactory dbContextFactory)
        {
            _dbContext = dbContextFactory.Create();
        }

        /// <summary>
        /// Запрос на получение постов пользователя
        /// </summary>
        /// <param name="id">Идентификатор пользователя</param>
        /// <returns></returns>
        /// <response code="200">Запрос выполнен успешно</response>
        /// <response code="400">Не валидные входные параметры</response>
        /// <response code="404">Пользователь не найден</response>
        /// <response code="500">Внутрення ошибка сервиса</response>
        [HttpGet(ApiRouters.Users.GetUserPosts)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<GetUserPostsResponseData>>> GetPosts([FromRoute] Guid id)
        {
            try
            {
                if (!id.IsValidIdentifier()) return BadRequest(ErrorDescription.InvalidIdentifier);

                var user = await _dbContext.Users.FindAsync(id);
                if (user is null) return NotFound(ErrorDescription.UserNotFound);

                var authUser = HttpContext.GetAuthenticatedUserInfo();

                var banEntity =
                    await _dbContext.BanList.FirstOrDefaultAsync(x => x.WhoId == user.Id && x.WhomId == authUser.Id);

                if (banEntity != null) return BadRequest("Вы забанены пользователем.");

                var posts = await _dbContext.Posts
                    .Include(x => x.PostComments)
                        .ThenInclude(x => x.User)
                    .Include(x => x.PostComments)
                        .ThenInclude(x => x.CommentLikes)
                            .ThenInclude(x => x.User)
                    .Include(x => x.PostLikes)
                        .ThenInclude(x => x.User)
                    .Where(x => x.UserId == user.Id)
                    .OrderByDescending(x => x.CreateDate)
                    .ToListAsync();

                var response = new List<GetUserPostsResponseData>();

                posts.ForEach(post =>
                {
                    var postComments = post.PostComments
                        .Where(comment => comment.ParentId == Guid.Empty)
                        .Select(comment => new PostComment
                        {
                            Id = comment.Id,
                            UserName = comment.User.UserName,
                            Comment = comment.Comment,
                            Likers = comment.CommentLikes.Select(like => like.User.UserName).ToList(),
                            LikesCount = comment.CommentLikes.Count,
                            Answers = new List<PostComment>()
                        }).ToList();

                    post.PostComments
                        .ToList()
                        .RemoveAll(postComment => postComments.Exists(x => x.Id == postComment.Id));

                    postComments.ForEach(postComment =>
                    {
                        SearchAnswers(postComment, post.PostComments.ToList());
                    });

                    response.Add(new GetUserPostsResponseData()
                    {
                        Id = post.Id,
                        Post = post.Post,
                        LikeCount = post.UserId != user.Id ? post.PostLikes.Count : 0,
                        LikeUsers = post.UserId == user.Id
                            ? post.PostLikes.Select(like => like.User.UserName).ToList()
                            : new List<string>(),
                        Comments = postComments
                    });
                });

                return response;
            }
            catch (Exception e)
            {
                WebApiLogger.LogException(e);
                return StatusCode(StatusCodes.Status500InternalServerError, ErrorDescription.InternalServerError);
            }
        }

        private void SearchAnswers(PostComment parentComment, List<PostComments> comments)
        {
            var answers = comments
                .Where(comment => comment.ParentId == parentComment.Id)
                .Select(comment => new PostComment
                {
                    Id = comment.Id,
                    UserName = comment.User.UserName,
                    Comment = comment.Comment,
                    Likers = comment.CommentLikes.Select(like => like.User.UserName).ToList(),
                    LikesCount = comment.CommentLikes.Count,
                    Answers = new List<PostComment>()
                })
                .ToList();

            parentComment.Answers.AddRange(answers);
            comments.RemoveAll(comment => answers.Exists(answer => answer.Id == comment.Id));

            parentComment.Answers.ForEach(answer => SearchAnswers(answer, comments));
        }

        /// <summary>
        /// Запрос добавления пользователя в бан лист
        /// </summary>
        /// <param name="id">Идентификатор пользователя</param>
        /// <returns></returns>
        /// <response code="200">Запрос выполнен успешно</response>
        /// <response code="400">Не валидные входные параметры</response>
        /// <response code="404">Пользователь не найден</response>
        /// <response code="500">Внутрення ошибка сервиса</response>
        [HttpPost(ApiRouters.Users.AddUserBanList)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> AddUserBanList([FromRoute] Guid id)
        {
            try
            {
                if (!id.IsValidIdentifier()) return BadRequest(ErrorDescription.InvalidIdentifier);

                var whomUser = await _dbContext.Users.FindAsync(id);
                if (whomUser is null) return NotFound(ErrorDescription.UserNotFound);

                var whoUser = HttpContext.GetAuthenticatedUserInfo();

                var newUserBan = new BanList
                {
                    Id = Guid.NewGuid(),
                    WhoId = whoUser.Id,
                    WhomId = whomUser.Id
                };

                _dbContext.Entry(newUserBan).State = EntityState.Added;
                await _dbContext.SaveChangesAsync();

                return Ok();
            }
            catch (Exception e)
            {
                WebApiLogger.LogException(e);
                return StatusCode(StatusCodes.Status500InternalServerError, ErrorDescription.InternalServerError);
            }
        }

        /// <summary>
        /// Запрос на удаления пользователя из бан листа
        /// </summary>
        /// <param name="id">Идентификатор пользователя</param>
        /// <returns></returns>
        /// <response code="200">Запрос выполнен успешно</response>
        /// <response code="400">Не валидные входные параметры</response>
        /// <response code="404">Пользователь не найден</response>
        /// <response code="500">Внутрення ошибка сервиса</response>
        [HttpDelete(ApiRouters.Users.DeleteUserBanList)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteUserBanList([FromRoute] Guid id)
        {
            try
            {
                if (!id.IsValidIdentifier()) return BadRequest(ErrorDescription.InvalidIdentifier);

                var whomUser = await _dbContext.Users.FindAsync(id);
                if (whomUser is null) return NotFound(ErrorDescription.UserNotFound);

                var whoUser = HttpContext.GetAuthenticatedUserInfo();

                var userBan = await _dbContext.BanList.FirstOrDefaultAsync(x => x.WhoId == whoUser.Id && x.WhomId == whomUser.Id);
                if (userBan is null) return NotFound("Пользователь не найден в бан листе.");

                _dbContext.Entry(userBan).State = EntityState.Deleted;
                await _dbContext.SaveChangesAsync();

                return Ok();
            }
            catch (Exception e)
            {
                WebApiLogger.LogException(e);
                return StatusCode(StatusCodes.Status500InternalServerError, ErrorDescription.InternalServerError);
            }
        }
    }
}