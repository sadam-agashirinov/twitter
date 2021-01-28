using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using TwitterApi.Core.Contracts.Account;
using TwitterApi.Core.Contracts.Common;
using TwitterApi.DataLayer.Common;
using TwitterApi.DataLayer.Entities.Models;
using TwitterApi.DataLayer.Settings;
using TwitterApi.DataLayer.Utils;
using WebApi.DataLayer.Extensions;

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
        private readonly TwitterDbContext _dbContext;

        /// <summary>
        /// Контроллер авторизации и регистрации аккаунта
        /// </summary>
        public AccountController(DbContextFactory dbContextFactory)
        {
            _dbContext = dbContextFactory.Create();
        }

        /// <summary>
        /// Запрос регистрация нового пользователя
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Запрос выполнен успешно</response>
        /// <response code="400">
        /// Не валидные входные параметры.
        /// Пользователь с таким логином уже существует.
        /// Ошибка генерации токена.</response>
        /// <response code="500">Ошибка выполнения запроса</response>
        [HttpPost(ApiRouters.Account.Registration)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<RegistrationResponseData>> Registration([FromForm] RegistrationRequestData requestData)
        {
            try
            {
                var validationResult = await requestData.ValidateAsync();
                if (!validationResult.IsValid) return BadRequest(validationResult.Errors);

                var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Login == requestData.Login.ToLower());
                if (user != null) return BadRequest(ErrorDescription.UserLoginExist);

                var newUser = new Users
                {
                    Id = Guid.NewGuid(),
                    Login = requestData.Login.ToLower(),
                    Password = DataUtils.Encrypt(requestData.Password),
                    UserName = requestData.UserName,
                    Email = requestData.Email
                };

                var claimsPrincipal = JwtTokenUtils.CreateClaimsPrincipal(newUser);
                var token = JwtTokenUtils.GenerateAccessToken(claimsPrincipal);
                var refreshToken = JwtTokenUtils.GenerateRefreshToken();

                if (!ValidationUtils.IsValidString(token) || !ValidationUtils.IsValidString(refreshToken))
                    return BadRequest("Ошибка генерации токена.");

                var newRefreshToken = new Tokens
                {
                    Id = Guid.NewGuid(),
                    UserId = newUser.Id,
                    RefreshToken = refreshToken,
                    ExpireTime = DateTime.UtcNow.AddDays(JwtSettings.Instance.RefreshTokenLifeTime),
                    Used = false
                };

                _dbContext.Entry(newUser).State = EntityState.Added;
                _dbContext.Entry(newRefreshToken).State = EntityState.Added;
                await _dbContext.SaveChangesAsync();

                return new RegistrationResponseData
                {
                    Id = newUser.Id,
                    AccessToken = token,
                    RefreshToken = refreshToken
                };
            }
            catch (Exception e)
            {
                WebApiLogger.LogException(e);
                return StatusCode(StatusCodes.Status500InternalServerError, ErrorDescription.InternalServerError);
            }
        }

        /// <summary>
        /// Запрос авторизация пользователя
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Запрос выполнен успешно</response>
        /// <response code="400">
        /// Не валидные входные параметры.
        /// Неверный пароль.
        /// Ошибка генерации токена.</response>
        /// <response code="404">Пользователь не найден</response>
        /// <response code="500">Ошибка выполнения запроса</response>
        [HttpPost(ApiRouters.Account.Authenticate)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<AuthenticateResponseData>> Authenticate([FromForm] AuthenticateRequestData requestData)
        {
            try
            {
                var validationResult = await requestData.ValidateAsync();
                if (!validationResult.IsValid) return BadRequest(validationResult.Errors);

                var user = await _dbContext.Users.FirstOrDefaultAsync(x =>
                    x.Login.ToLower() == requestData.Login.ToLower());

                if (user is null) return NotFound(ErrorDescription.UserNotFound);

                if (user.Password != DataUtils.Encrypt(requestData.Password))
                    return BadRequest(ErrorDescription.UserPasswordIncorrect);

                var claimsPrincipal = JwtTokenUtils.CreateClaimsPrincipal(user);
                var token = JwtTokenUtils.GenerateAccessToken(claimsPrincipal);
                var refreshToken = JwtTokenUtils.GenerateRefreshToken();

                if (!ValidationUtils.IsValidString(token) || !ValidationUtils.IsValidString(refreshToken))
                    return BadRequest("Ошибка генерации токена.");

                var newRefreshToken = new Tokens
                {
                    Id = Guid.NewGuid(),
                    UserId = user.Id,
                    RefreshToken = refreshToken,
                    ExpireTime = DateTime.UtcNow.AddDays(JwtSettings.Instance.RefreshTokenLifeTime),
                    Used = false
                };

                _dbContext.Entry(newRefreshToken).State = EntityState.Added;
                await _dbContext.SaveChangesAsync();

                return new AuthenticateResponseData
                {
                    Id = user.Id,
                    AccessToken = token,
                    RefreshToken = refreshToken
                };
            }
            catch (Exception e)
            {
                WebApiLogger.LogException(e);
                return StatusCode(StatusCodes.Status500InternalServerError, ErrorDescription.InternalServerError);
            }
        }

        /// <summary>
        /// Запрос обновления токена
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Запрос выполнен успешно</response>
        /// <response code="400">
        /// Не валидные входные параметры.
        /// Не валидный токен
        /// Ошибка генерации токена.</response>
        /// <response code="404">Токен обновления не найден</response>
        /// <response code="500">Ошибка выполнения запроса</response>
        [HttpPost(ApiRouters.Account.RefreshToken)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<RefreshTokenResponseData>> RefreshToken([FromForm] RefreshTokenRequestData requestData)
        {
            try
            {
                var validator = await requestData.ValidateAsync();
                if (!validator.IsValid) return BadRequest(validator.Errors);

                if (!JwtTokenUtils.ValidateToken(requestData.AccessToken)) return BadRequest(ErrorDescription.InvalidAccessToken);

                var authUser = JwtTokenUtils.CreateAuthenticatedUserInfo(requestData.AccessToken);

                var refreshToken =
                    await _dbContext.Tokens.SingleOrDefaultAsync(x =>
                        x.UserId == authUser.Id && x.RefreshToken == requestData.RefreshToken && !x.Used);

                if (refreshToken is null) return NotFound(ErrorDescription.RefreshTokenNotFound);
                if (refreshToken.IsExpired()) return BadRequest(ErrorDescription.RefreshTokenExpired);

                var principal = JwtTokenUtils.GetClaimsPrincipal(requestData.AccessToken);
                var newAccessToken = JwtTokenUtils.GenerateAccessToken(principal);
                var newRefreshToken = JwtTokenUtils.GenerateRefreshToken();

                if (!ValidationUtils.IsValidString(newAccessToken) || !ValidationUtils.IsValidString(newRefreshToken))
                    return BadRequest("Ошибка генерации токена.");

                var newRefreshTokenEntity = new Tokens
                {
                    Id = Guid.NewGuid(),
                    UserId = authUser.Id,
                    RefreshToken = newRefreshToken,
                    ExpireTime = DateTime.Now.AddDays(JwtSettings.Instance.RefreshTokenLifeTime),
                    Used = false
                };

                refreshToken.Used = true;

                _dbContext.Entry(newRefreshTokenEntity).State = EntityState.Added;
                _dbContext.Entry(refreshToken).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();

                return new RefreshTokenResponseData
                {
                    AccessToken = newAccessToken,
                    RefreshToken = newRefreshToken
                };
            }
            catch (Exception e)
            {
                WebApiLogger.LogException(e);
                return StatusCode(StatusCodes.Status500InternalServerError, ErrorDescription.InternalServerError);
            }
        }
    }
}