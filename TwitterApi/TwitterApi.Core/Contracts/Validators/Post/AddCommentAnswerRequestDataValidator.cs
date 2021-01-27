using FluentValidation;
using System;
using TwitterApi.Core.Contracts.Post;

namespace TwitterApi.Core.Contracts.Validators.Post
{
    /// <summary>
    /// Валидатор данных запроса добавления комментария на комментарий
    /// </summary>
    public class AddCommentAnswerRequestDataValidator : AbstractValidator<AddCommentAnswerRequestData>
    {
        /// <summary>
        /// Валидатор данных запроса добавления комментария на комментарий
        /// </summary>
        public AddCommentAnswerRequestDataValidator()
        {
            RuleFor(x => x.CommentId)
                .NotEqual(Guid.Empty)
                .WithMessage("Неверный идентификатор комментария.");

            RuleFor(x => x.Comment)
                .NotEmpty()
                .WithMessage("Комментарий обязателен для заполнения.");
        }
    }
}