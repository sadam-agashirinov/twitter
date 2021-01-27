using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private TwitterDbContext _dbContext;
        /// <summary>
        /// Контроллер для работы с постами
        /// </summary>
        public PostsController(TwitterDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Добавление нового поста
        /// </summary>
        /// <returns></returns>
        [HttpPost(ApiRouters.Post.AddPost)]
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
        /// Добавить комментарий к посту
        /// </summary>
        /// <returns></returns>
        [HttpPost(ApiRouters.Post.AddPostComment)]
        public async Task<ActionResult> AddPostComment([FromRoute] Guid id, [FromForm] AddPostCommentRequestData requestData)
        {
            try
            {
                var validationResult = await requestData.ValidateAsync();
                if (!id.IsValidIdentifier() || !validationResult.IsValid)
                    return BadRequest(validationResult.IsValid
                        ? "Невалидный идентификатор."
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
    }
}