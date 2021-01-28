﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TwitterApi.Core.Contracts.Common;
using TwitterApi.Core.Contracts.Post;
using TwitterApi.DataLayer.Common;
using TwitterApi.DataLayer.Entities.Models;
using TwitterApi.DataLayer.Extensions;
using TwitterApi.DataLayer.Utils;

namespace TwitterApi.Core.Controllers
{
    /// <summary>
    /// Контроллер для работы с постами
    /// </summary>
    [ApiController]
    [Authorize]
    [Produces("application/json")]
    public class PostsController : ControllerBase
    {
        private readonly TwitterDbContext _dbContext;
        /// <summary>
        /// Контроллер для работы с постами
        /// </summary>
        public PostsController(DbContextFactory dbContextFactory)
        {
            _dbContext = dbContextFactory.Create();
        }

        /// <summary>
        /// Запрос на добавление нового поста
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Запрос выполнен успешно</response>
        /// <response code="500">Внутренняя ошибика сервиса</response>
        [HttpPost(ApiRouters.Posts.AddPost)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> AddPost([FromForm] AddPostRequestData requestData)
        {
            try
            {
                var validationResult = await requestData.ValidateAsync();
                if (!validationResult.IsValid) return BadRequest(validationResult.Errors);

                var user = HttpContext.GetAuthenticatedUserInfo();

                var newPost = new Posts
                {
                    Id = Guid.NewGuid(),
                    Post = requestData.Post,
                    UserId = user.Id,
                    CreateDate = DateTime.Now
                };

                _dbContext.Entry(newPost).State = EntityState.Added;
                await _dbContext.SaveChangesAsync();

                return Ok(newPost.Id);
            }
            catch (Exception e)
            {
                WebApiLogger.LogException(e);
                return StatusCode(StatusCodes.Status500InternalServerError, ErrorDescription.InternalServerError);
            }
        }

        /// <summary>
        /// Запрос получения ленты постов
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Запрос выполнен успешно</response>
        /// <response code="500">Внутренняя ошибика сервиса</response>
        [HttpGet(ApiRouters.Posts.GetPosts)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<GetPostsResponseData>>> GetPosts()
        {
            try
            {
                var user = HttpContext.GetAuthenticatedUserInfo();

                var posts = await _dbContext.Posts
                    .Include(x => x.PostLikes)
                        .ThenInclude(x => x.User)
                    .Include(x => x.User.BanListWho)
                    .Include(x => x.User.BanListWhom)
                    .Where(x => x.User.BanListWho.Any(z => z.WhoId == user.Id) ||
                                x.User.BanListWhom.Any(z => z.WhomId == user.Id))
                    .OrderByDescending(x => x.CreateDate)
                    .ToListAsync();

                return posts.Select(post => new GetPostsResponseData
                {
                    Id = post.Id,
                    Post = post.Post,
                    LikeCount = post.UserId != user.Id ? post.PostLikes.Count : 0,
                    LikeUsers = post.UserId == user.Id
                        ? post.PostLikes.Select(x => x.User.UserName).ToList()
                        : new List<string>()
                }).ToList();
            }
            catch (Exception e)
            {
                WebApiLogger.LogException(e);
                return StatusCode(StatusCodes.Status500InternalServerError, ErrorDescription.InternalServerError);
            }
        }

        /// <summary>
        /// Запрос добавления комментария к посту
        /// </summary>
        /// <param name="id">Идентификатор поста</param>
        /// <param name="requestData">Данные запроса</param>
        /// <response code="200">Запрос выполнен успешно</response>
        /// <response code="400">Не валидные входные параметры</response>
        /// <response code="404">Пост не найден</response>
        /// <response code="500">Внутренняя ошибика сервиса</response>
        /// <returns></returns>
        [HttpPost(ApiRouters.Posts.AddPostComment)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> AddPostComment([FromRoute] Guid id, [FromForm] AddPostCommentRequestData requestData)
        {
            try
            {
                var validationResult = await requestData.ValidateAsync();
                if (!id.IsValidIdentifier() || !validationResult.IsValid)
                    return BadRequest(validationResult.IsValid
                        ? ErrorDescription.InvalidIdentifier
                        : validationResult.Errors.ToString());


                var post = await _dbContext.Posts.FindAsync(id);
                if (post is null) return NotFound(ErrorDescription.PostNotFound);

                var user = HttpContext.GetAuthenticatedUserInfo();

                var newPostComment = new PostComments
                {
                    Id = Guid.NewGuid(),
                    PostId = id,
                    Comment = requestData.Comment,
                    UserId = user.Id,
                    ParentId = Guid.Empty
                };

                _dbContext.Entry(newPostComment).State = EntityState.Added;
                await _dbContext.SaveChangesAsync();

                return Ok(newPostComment.Id);
            }
            catch (Exception e)
            {
                WebApiLogger.LogException(e);
                return StatusCode(StatusCodes.Status500InternalServerError, ErrorDescription.InternalServerError);
            }
        }

        /// <summary>
        /// Запрос добавления комментария к комментарию в посте
        /// </summary>
        /// <param name="id">Идентификатор поста</param>
        /// <param name="requestData">Данные запроса</param>
        /// <returns></returns>
        /// <response code="200">Запрос выполнен успешно</response>
        /// <response code="400">Не валидные входные параметры</response>
        /// <response code="404">Пост или комментарий не найдены</response>
        /// <response code="500">Внутренняя ошибика сервиса</response>
        [HttpPost(ApiRouters.Posts.AddAnswerComment)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> AddCommentAnswer([FromRoute] Guid id, [FromForm] AddCommentAnswerRequestData requestData)
        {
            try
            {
                var validationResult = await requestData.ValidateAsync();
                if (!id.IsValidIdentifier() || !validationResult.IsValid)
                    return BadRequest(validationResult.IsValid
                        ? "Невалидный идентификатор поста."
                        : validationResult.Errors.ToString());

                var post = await _dbContext.Posts.FindAsync(id);
                if (post is null) return NotFound(ErrorDescription.PostNotFound);

                var comment = await _dbContext.PostComments.FindAsync(requestData.CommentId);
                if (comment is null) return NotFound(ErrorDescription.CommentNotFound);

                var user = HttpContext.GetAuthenticatedUserInfo();

                var newCommentAnswer = new PostComments
                {
                    Id = Guid.NewGuid(),
                    UserId = user.Id,
                    Comment = requestData.Comment,
                    ParentId = requestData.CommentId,
                    PostId = id
                };

                _dbContext.Entry(newCommentAnswer).State = EntityState.Added;
                await _dbContext.SaveChangesAsync();

                return Ok(newCommentAnswer.Id);
            }
            catch (Exception e)
            {
                WebApiLogger.LogException(e);
                return StatusCode(StatusCodes.Status500InternalServerError, ErrorDescription.InternalServerError);
            }
        }

        /// <summary>
        /// Запрос добавления лайка к посту
        /// </summary>
        /// <param name="id">Идентификатор поста</param>
        /// <returns></returns>
        /// <response code="200">Запрос выполнен успешно</response>
        /// <response code="400">Не валидные входные параметры</response>
        /// <response code="404">Пост не найдены</response>
        /// <response code="500">Внутренняя ошибика сервиса</response>
        [HttpPost(ApiRouters.Posts.AddLikePost)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> AddLikePost(Guid id)
        {
            try
            {
                if (!id.IsValidIdentifier()) return BadRequest(ErrorDescription.InvalidIdentifier);

                var post = await _dbContext.Posts.FindAsync(id);
                if (post is null) return NotFound(ErrorDescription.PostNotFound);

                var user = HttpContext.GetAuthenticatedUserInfo();

                var newPostLike = new PostLikes
                {
                    Id = Guid.NewGuid(),
                    PostId = post.Id,
                    UserId = user.Id
                };

                _dbContext.Entry(newPostLike).State = EntityState.Added;
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
        /// Запрос добавления лайка к комментарию
        /// </summary>
        /// <param name="id">Идентификатор комментария</param>
        /// <returns></returns>
        /// <response code="200">Запрос выполнен успешно</response>
        /// <response code="400">Не валидные входные параметры</response>
        /// <response code="404">Комментарий не найдены</response>
        /// <response code="500">Внутренняя ошибика сервиса</response>
        [HttpPost(ApiRouters.Posts.AddLikeComment)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> AddLikeComment(Guid id)
        {
            try
            {
                if (!id.IsValidIdentifier()) return BadRequest(ErrorDescription.InvalidIdentifier);

                var comment = await _dbContext.PostComments.FindAsync(id);
                if (comment is null) return NotFound(ErrorDescription.CommentNotFound);

                var user = HttpContext.GetAuthenticatedUserInfo();

                var newCommentLike = new CommentLikes
                {
                    Id = Guid.NewGuid(),
                    PostCommentId = comment.Id,
                    UserId = user.Id
                };

                _dbContext.Entry(newCommentLike).State = EntityState.Added;
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