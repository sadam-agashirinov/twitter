namespace TwitterApi.Core.Contracts.Common
{
    /// <summary>
    /// Маршруты для запросов
    /// </summary>
    public static class ApiRouters
    {
        public const string Root =  "api";
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
    }
}