namespace TwitterApi.DataLayer.Utils
{
    public class ErrorDescription
    {
        public const string InvalidInputParameters = "Не правильное значение входных параметров.";
        public const string DbQueryExecutionError = "Ошибка выполнения запроса к БД.";
        public const string InternalServerError = "Внутренняя ошибка сервиса.";

        public const string UserNotFound = "Пользователь не найден.";
        public const string UserPasswordIncorrect = "Неверный пароль.";
        public const string UserLoginExist = "Пользователь с таким логином существует.";

        public const string InvalidAccessToken = "Не валидный токен.";
        public const string RefreshTokenNotFound = "Токен обновления не найден.";
        public const string RefreshTokenExpired = "Истек срок токена обновления.";
    }
}
