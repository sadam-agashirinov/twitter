using FluentValidation;
using TwitterApi.Core.Contracts.Post;

namespace TwitterApi.Core.Contracts.Validators.Post
{
    /// <summary>
    /// Валидатор данных запроса добавления комментария к посту
    /// </summary>
    public class AddPostCommentRequestDataValidator : AbstractValidator<AddPostCommentRequestData>
    {
        /// <summary>
        /// Валидатор данных запроса добавления комментария к посту
        /// </summary>
        public AddPostCommentRequestDataValidator()
        {
            RuleFor(x => x.Comment)
                .NotEmpty()
                .WithMessage("Комментарий обязателен для заполнения.");
        }
    }
}