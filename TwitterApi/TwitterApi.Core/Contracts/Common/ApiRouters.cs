namespace TwitterApi.Core.Contracts.Common
{
    /// <summary>
    /// Маршруты для запросов
    /// </summary>
    public static class ApiRouters
    {
        public const string Root = "api";
        /// <summary>
        /// Базовый путь маршрута
        /// </summary>
        public const string Base = Root + "/[controller]";

        /// <summary>
        /// Маршруты для методов авторизации
        /// </summary>
        public static class Account
        {
            /// <summary>
            /// Запрос регистрация пользователя
            /// </summary>
            public const string Registration = Base + "/registration";
            /// <summary>
            /// Запрос авторизация пользователя
            /// </summary>
            public const string Authenticate = Base + "/authenticate";
            /// <summary>
            /// Запрос обновление токена
            /// </summary>
            public const string RefreshToken = Base + "/refreshtoken";
        }

        /// <summary>
        /// Маршруты для работы постом
        /// </summary>
        public static class Posts
        {
            /// <summary>
            /// Запрос на добавление нового поста
            /// </summary>
            public const string AddPost = Base;
            /// <summary>
            /// Запрос получения ленты постов
            /// </summary>
            public const string GetPosts = Base;
            /// <summary>
            /// Запрос добавления комментария к посту
            /// </summary>
            public const string AddPostComment = Base + "/{id}/comment";
            /// <summary>
            /// Запрос добавления комментария к комметарию в посте
            /// </summary>
            public const string AddAnswerComment = Base + "/{id}/comments/answer";
            /// <summary>
            /// Запрос добавления лайка к посту
            /// </summary>
            public const string AddLikePost = Base + "/{id}/like";
            /// <summary>
            /// Запрос добавления лайка к комментарию
            /// </summary>
            public const string AddLikeComment = Base + "/comments/{id}/like";
        }

        /// <summary>
        /// Маршруты для работы с пользователями
        /// </summary>
        public static class Users
        {
            /// <summary>
            /// Запрос на получение постов пользователя
            /// </summary>
            public const string GetUserPosts = Base + "/{id}/posts";
            /// <summary>
            /// Запрос добавления пользователя в бан лист
            /// </summary>
            public const string AddUserBanList = Base + "/{id}/ban";
            /// <summary>
            /// Запрос на удаления пользователя из бан листа
            /// </summary>
            public const string DeleteUserBanList = Base + "/{id}/ban";
        }
    }
}