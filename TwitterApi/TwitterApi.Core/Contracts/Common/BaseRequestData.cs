using FluentValidation.Results;
using System.Threading.Tasks;

namespace TwitterApi.Core.Contracts.Common
{
    /// <summary>
    /// Базовый класс для данных запроса
    /// </summary>
    public class BaseRequestData
    {
        /// <summary>
        /// Проверка валидности данных
        /// </summary>
        /// <returns></returns>
        public virtual async Task<ValidationResult> ValidateAsync()
        {
            return new ValidationResult();
        }
    }
}