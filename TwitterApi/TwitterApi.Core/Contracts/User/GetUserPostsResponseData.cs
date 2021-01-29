using System;
using System.Collections.Generic;
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
        /// <summary>
        /// Количество лайков
        /// </summary>
        public int LikeCount { get; set; }
        /// <summary>
        /// Пользователи поставившие лайк на пост
        /// </summary>
        public List<string> LikeUsers { get; set; }
        /// <summary>
        /// Комментарии к посту
        /// </summary>
        public List<PostComment> Comments { get; set; }
    }
}