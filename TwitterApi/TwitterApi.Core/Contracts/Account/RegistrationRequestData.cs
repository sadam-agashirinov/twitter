using FluentValidation.Results;
using System.Threading.Tasks;
using TwitterApi.Core.Contracts.Common;
using TwitterApi.Core.Contracts.Validators.Account;

namespace TwitterApi.Core.Contracts.Account
{
    /// <summary>
    /// Данные запроса регистрации
    /// </summary>
    public class RegistrationRequestData : BaseRequestData
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
        /// Имя
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// Адрес электронной почты
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Проверка валидности данных
        /// </summary>
        /// <returns></returns>
        public override async Task<ValidationResult> ValidateAsync()
        {
            var validator = new RegistrationRequestDataValidator();
            return await validator.ValidateAsync(this);
        }
    }
}