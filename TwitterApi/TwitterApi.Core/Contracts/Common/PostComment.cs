using System;
using System.Collections.Generic;

namespace TwitterApi.Core.Contracts.Common
{
    /// <summary>
    /// Комментарий к посту
    /// </summary>
    public class PostComment
    {
        /// <summary>
        /// Идентификатор комментария
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Имя пользователя написавшего комментарий
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// Текст комментария
        /// </summary>
        public string Comment { get; set; }
        /// <summary>
        /// Пользователи поставившие лайк на коммент
        /// </summary>
        public List<string> Likers { get; set; }
        /// <summary>
        /// Количество лайков комментария
        /// </summary>
        public int LikesCount { get; set; }
        /// <summary>
        /// Ответы на комментарии
        /// </summary>
        public List<PostComment> Answers { get; set; }
    }
}