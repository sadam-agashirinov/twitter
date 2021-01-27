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
        private TwitterDbContext _dbContext;

        /// <summary>
        /// Контроллер для работы с пользователями
        /// </summary>
        public UsersController(TwitterDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Получить посты пользователя
        /// </summary>
        /// <param name="id">Идентификатор пользователя</param>
        /// <returns></returns>
        [HttpGet(ApiRouters.Users.GetUserPosts)]
        public async Task<ActionResult<IEnumerable<GetUserPostsResponseData>>> GetPosts([FromRoute] Guid id)
        {
            try
            {
                if (!id.IsValidIdentifier()) return BadRequest(ErrorDescription.InvalidIdentifier);

                var user = await _dbContext.Users.FindAsync(id);
                if (user is null) return NotFound(ErrorDescription.UserNotFound);

                var posts = await _dbContext.Posts
                    .Where(x => x.UserId == user.Id)
                    .OrderByDescending(x=>x.CreateDate)
                    .ToListAsync();

                return posts.Select(post => new GetUserPostsResponseData
                {
                    Id = post.Id,
                    Post = post.Post
                }).ToList();
            }
            catch (Exception e)
            {
                WebApiLogger.LogException(e);
                return StatusCode(StatusCodes.Status500InternalServerError, ErrorDescription.InternalServerError);
            }
        }
    }
}