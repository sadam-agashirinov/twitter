namespace TwitterApi.Core.Contracts.Common
{
    /// <summary>
    /// Маршруты для запросов
    /// </summary>
    public static class ApiRouters
    {
        public const string Root = "api";
        public const string Base = Root + "/[controller]";

        /// <summary>
        /// Маршруты для методов авторизации
        /// </summary>
        public static class Account
        {
            /// <summary>
            /// Регистрация пользователя
            /// </summary>
            public const string Registration = Base + "/registration";
            /// <summary>
            /// Авторизация пользователя
            /// </summary>
            public const string Authenticate = Base + "/authenticate";
            /// <summary>
            /// Обновление токена
            /// </summary>
            public const string RefreshToken = Base + "/refreshtoken";
        }

        /// <summary>
        /// Маршруты для работы постом
        /// </summary>
        public static class Post
        {
            /// <summary>
            /// Добавление нового поста
            /// </summary>
            public const string AddPost = Base;
            /// <summary>
            /// Добавить комментарий к посту
            /// </summary>
            public const string AddPostComment = Base + "/{id}/comment";
            /// <summary>
            /// Ответить на комметарий в посте
            /// </summary>
            public const string AddAnswerComment = Base + "/comments/{id}/answer";
            /// <summary>
            /// Добавить лайк к посту
            /// </summary>
            public const string AddLikePost = Base + "/{id}/like";
        }
    }
}