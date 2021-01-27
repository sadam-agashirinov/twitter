using FluentValidation;
using TwitterApi.Core.Contracts.Post;

namespace TwitterApi.Core.Contracts.Validators.Post
{
    /// <summary>
    /// Валидатор данных запроса добавления нового поста
    /// </summary>
    public class AddPostRequestDataValidator : AbstractValidator<AddPostRequestData>
    {
        /// <summary>
        /// Валидатор данных запроса добавления нового поста
        /// </summary>
        public AddPostRequestDataValidator()
        {
            RuleFor(x => x.Post)
                .NotEmpty()
                .WithMessage("Текст поста обязателен для заполнения.")
                .MaximumLength(255)
                .WithMessage("Текст поста превыщает допустимое количество символов (255).");
        }
    }
}