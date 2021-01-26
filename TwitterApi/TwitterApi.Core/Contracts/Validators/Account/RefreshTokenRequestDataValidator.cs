using FluentValidation;
using TwitterApi.Core.Contracts.Account;

namespace TwitterApi.Core.Contracts.Validators.Account
{
    /// <summary>
    /// Валидатор данных запроса обновления токена доступа
    /// </summary>
    public class RefreshTokenRequestDataValidator : AbstractValidator<RefreshTokenRequestData>
    {
        /// <summary>
        /// Валидатор данных запроса обновления токена доступа
        /// </summary>
        public RefreshTokenRequestDataValidator()
        {
            RuleFor(x => x.AccessToken)
                .NotEmpty()
                .WithMessage("Токен авторизации обязателен для заполнения.");

            RuleFor(x => x.RefreshToken)
                .NotEmpty()
                .WithMessage("Токена обновления обязателен для заполнения.");
        }
    }
}