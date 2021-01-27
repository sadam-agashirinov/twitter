using FluentValidation.Results;
using System.Threading.Tasks;
using TwitterApi.Core.Contracts.Common;
using TwitterApi.Core.Contracts.Validators.Post;

namespace TwitterApi.Core.Contracts.Post
{
    /// <summary>
    /// Данные запроса добавления нового поста
    /// </summary>
    public class AddPostRequestData : BaseRequestData
    {
        /// <summary>
        /// Текст поста
        /// </summary>
        public string Post { get; set; }

        /// <summary>
        /// Проверка валидности данных
        /// </summary>
        /// <returns></returns>
        public override async Task<ValidationResult> ValidateAsync()
        {
            var validator = new AddPostRequestDataValidator();
            return await validator.ValidateAsync(this);
        }
    }
}