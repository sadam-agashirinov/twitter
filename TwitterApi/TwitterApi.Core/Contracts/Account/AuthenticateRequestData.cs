using System.Threading.Tasks;
using FluentValidation.Results;
using TwitterApi.Core.Contracts.Common;
using TwitterApi.Core.Contracts.Validators.Account;

namespace TwitterApi.Core.Contracts.Account
{
    /// <summary>
    /// Данные запроса авторизации
    /// </summary>
    public class AuthenticateRequestData : BaseRequestData
    {
        /// <summary>
        /// Логин
        /// </summary>
        public string Login { get; set; }
        /// <summary>
        /// Пароль
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Проверка валидности данных
        /// </summary>
        /// <returns></returns>
        public override async Task<ValidationResult> ValidateAsync()
        {
            var validator = new AuthenticateRequestDataValidator();
            return await validator.ValidateAsync(this);
        }
    }
}