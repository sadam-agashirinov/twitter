using System;

namespace TwitterApi.Core.Contracts.Account
{
    /// <summary>
    /// Ответ на запроса авторизации пользователя
    /// </summary>
    public class AuthenticateResponseData
    {
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Токен доступа
        /// </summary>
        public string AccessToken { get; set; }
        /// <summary>
        /// Рефреш токен
        /// </summary>
        public string RefreshToken { get; set; }
    }
}