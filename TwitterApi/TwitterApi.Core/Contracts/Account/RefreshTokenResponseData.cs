using TwitterApi.Core.Contracts.Common;

namespace TwitterApi.Core.Contracts.Account
{
    /// <summary>
    /// Ответ на запрос обновления токена
    /// </summary>
    public class RefreshTokenResponseData : BaseResponseData
    {
        /// <summary>
        /// Токен доступа
        /// </summary>
        public string AccessToken { get; set; }
        /// <summary>
        /// Токен обновления
        /// </summary>
        public string RefreshToken { get; set; }
    }
}