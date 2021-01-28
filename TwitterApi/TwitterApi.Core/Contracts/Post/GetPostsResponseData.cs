﻿using System;
using System.Collections.Generic;
using TwitterApi.Core.Contracts.Common;

namespace TwitterApi.Core.Contracts.Post
{
    /// <summary>
    /// Ответ на запрос получения ленты постов
    /// </summary>
    public class GetPostsResponseData : BaseResponseData
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

        /// <summary>
        /// Комментарий к посту
        /// </summary>
        public class PostComment
        {
            /// <summary>
            /// Имя пользователя написавшего комментарий
            /// </summary>
            public string UserName { get; set; }
            /// <summary>
            /// Текст комментария
            /// </summary>
            public string Comment { get; set; }
            /// <summary>
            /// Ответы на комментарии
            /// </summary>
            public List<PostComment> Answers { get; set; }
        }
    }
}