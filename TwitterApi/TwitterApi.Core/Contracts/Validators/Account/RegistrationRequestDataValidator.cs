using FluentValidation;
using TwitterApi.Core.Contracts.Account;

namespace TwitterApi.Core.Contracts.Validators.Account
{
    /// <summary>
    /// Валидатор данных запроса регистрации
    /// </summary>
    public class RegistrationRequestDataValidator : AbstractValidator<RegistrationRequestData>
    {
        /// <summary>
        /// Валидатор данных запроса регистрации
        /// </summary>
        public RegistrationRequestDataValidator()
        {
            RuleFor(x => x.Login)
                .NotEmpty()
                .WithMessage("Логин обязателен для заполнения.")
                .MaximumLength(255)
                .WithMessage("Длина логина превыщает допустимое количество символов.");

            RuleFor(x=>x.Password)
                .NotEmpty()
                .WithMessage("Пароль обязателен для заполнения.")
                .MaximumLength(255)
                .WithMessage("Длина пароля превыщает допустимое количество символов.");

            RuleFor(x => x.UserName)
                .NotEmpty()
                .WithMessage("Имя обязателен для заполнения.")
                .MaximumLength(255)
                .WithMessage("Длина имя превыщает допустимое количество символов.");

            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Адрес электронной почты обязателен для заполнения.")
                .EmailAddress()
                .WithMessage("Некорректный адрес электронной почты.")
                .MaximumLength(255)
                .WithMessage("Длина адрес электронной почты превыщает допустимое количество символов.");
        }
    }
}