using System;
using TwitterApi.Core.Contracts.Common;

namespace TwitterApi.Core.Contracts.User
{
    /// <summary>
    /// Ответ на запрос всех постов пользователя
    /// </summary>
    public class GetUserPostsResponseData : BaseResponseData
    {
        /// <summary>
        /// Идентификатор поста
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Текст поста
        /// </summary>
        public string Post { get; set; }
    }
}