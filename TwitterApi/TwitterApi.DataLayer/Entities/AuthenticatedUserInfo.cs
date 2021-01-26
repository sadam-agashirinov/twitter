using System;

namespace TwitterApi.DataLayer.Entities
{
    /// <summary>
    /// Информация об авторизованном пользователе
    /// </summary>
    public class AuthenticatedUserInfo
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Имя
        /// </summary>
        public string UserName { get; set; }
    }
}