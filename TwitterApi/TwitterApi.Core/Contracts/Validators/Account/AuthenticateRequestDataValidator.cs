using FluentValidation;
using TwitterApi.Core.Contracts.Account;

namespace TwitterApi.Core.Contracts.Validators.Account
{
    /// <summary>
    /// Валидатор данных запроса авторизации
    /// </summary>
    public class AuthenticateRequestDataValidator : AbstractValidator<AuthenticateRequestData>
    {
        /// <summary>
        /// Валидатор данных запроса авторизации
        /// </summary>
        public AuthenticateRequestDataValidator()
        {
            RuleFor(x => x.Login)
                .NotEmpty()
                .WithMessage("Логин обязателен для заполнения.")
                .MaximumLength(255)
                .WithMessage("Длина логина превыщает допустимое количество символов.");

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("Пароль обязателен для заполнения.")
                .MaximumLength(255)
                .WithMessage("Длина пароля превыщает допустимое количество символов.");

        }
    }
}