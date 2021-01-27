using System;
using System.Threading.Tasks;
using FluentValidation.Results;
using TwitterApi.Core.Contracts.Common;
using TwitterApi.Core.Contracts.Validators.Post;

namespace TwitterApi.Core.Contracts.Post
{
    /// <summary>
    /// Данные запроса добавления коммнтарий к комментарию
    /// </summary>
    public class AddCommentAnswerRequestData : BaseRequestData
    {
        /// <summary>
        /// Идентификатор комментария на который пишется ответ
        /// </summary>
        public Guid CommentId { get; set; }
        /// <summary>
        /// Текст комментария
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// Проверка валидности данных
        /// </summary>
        /// <returns></returns>
        public override async Task<ValidationResult> ValidateAsync()
        {
            var validator = new AddCommentAnswerRequestDataValidator();
            return await validator.ValidateAsync(this);
        }
    }
}