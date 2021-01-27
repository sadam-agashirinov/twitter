using System.Threading.Tasks;
using FluentValidation.Results;
using TwitterApi.Core.Contracts.Common;
using TwitterApi.Core.Contracts.Validators.Post;

namespace TwitterApi.Core.Contracts.Post
{
    /// <summary>
    /// Данные для запроса добавления комментария к посту
    /// </summary>
    public class AddPostCommentRequestData : BaseRequestData
    {
        /// <summary>
        /// Комментарий
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// Проверка валидности данных
        /// </summary>
        /// <returns></returns>
        public override async Task<ValidationResult> ValidateAsync()
        {
            var validator = new AddPostCommentRequestDataValidator();
            return await validator.ValidateAsync(this);
        }
    }
}