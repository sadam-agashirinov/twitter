using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TwitterApi.Core.Contracts.Common;
using TwitterApi.DataLayer.Common;

namespace TwitterApi.Core.Controllers
{
    /// <summary>
    /// Контроллер авторизации и регистрации аккаунта
    /// </summary>
    [ApiController]
    [AllowAnonymous]
    [Produces("application/json")]
    public class AccountController : ControllerBase
    {
        private TwitterDbContext _dbContext;
        
        /// <summary>
        /// Контроллер авторизации и регистрации аккаунта
        /// </summary>
        public AccountController(TwitterDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Регистрация нового пользователя
        /// </summary>
        /// <returns></returns>
        [HttpPost(ApiRouters.Account.Registration)]
        public Task<ActionResult> Registration()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Авторизация пользователя
        /// </summary>
        /// <returns></returns>
        [HttpPost(ApiRouters.Account.Authenticate)]
        public Task<ActionResult> Authenticate()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Обновления рефреш токена
        /// </summary>
        /// <returns></returns>
        [HttpPost(ApiRouters.Account.RefreshToken)]
        public Task<ActionResult> RefreshToken()
        {
            throw new NotImplementedException();
        }
    }
}