using System.Threading.Tasks;
using FluentValidation.Results;
using TwitterApi.Core.Contracts.Common;
using TwitterApi.Core.Contracts.Validators.Account;

namespace TwitterApi.Core.Contracts.Account
{
    /// <summary>
    /// Данные запроса обновления токена доступа
    /// </summary>
    public class RefreshTokenRequestData : BaseRequestData
    {
        /// <summary>
        /// Токен доступа
        /// </summary>
        public string AccessToken { get; set; }
        /// <summary>
        /// Рефреш токен
        /// </summary>
        public string RefreshToken { get; set; }

        /// <summary>
        /// Проверка валидности данных
        /// </summary>
        /// <returns></returns>
        public override async Task<ValidationResult> ValidateAsync()
        {
            var validator = new RefreshTokenRequestDataValidator();
            return await validator.ValidateAsync(this);
        }
    }
}