using System;
using TwitterApi.Core.Contracts.Common;

namespace TwitterApi.Core.Contracts.Account
{
    /// <summary>
    /// Ответ на запрос регистрации нового пользователя
    /// </summary>
    public class RegistrationResponseData : BaseResponseData
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