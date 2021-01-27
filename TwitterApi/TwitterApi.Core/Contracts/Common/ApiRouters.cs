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
        public static class Posts
        {
            /// <summary>
            /// Добавление нового поста
            /// </summary>
            public const string AddPost = Base;
            /// <summary>
            /// Получить ленту постов
            /// </summary>
            public const string GetPosts = Base;
            /// <summary>
            /// Добавить комментарий к посту
            /// </summary>
            public const string AddPostComment = Base + "/{id}/comment";
            /// <summary>
            /// Ответить на комметарий в посте
            /// </summary>
            public const string AddAnswerComment = Base + "/{id}/comments/answer";
            /// <summary>
            /// Добавить лайк к посту
            /// </summary>
            public const string AddLikePost = Base + "/{id}/like";
            /// <summary>
            /// Добавить лайк к комментарию
            /// </summary>
            public const string AddLikeComment = Base + "/comments/{id}/like";
        }

        /// <summary>
        /// Маршруты для работы с пользователями
        /// </summary>
        public static class Users
        {
            /// <summary>
            /// Получить посты пользователя
            /// </summary>
            public const string GetUserPosts = Base + "/{id}/posts";
        }
    }
}