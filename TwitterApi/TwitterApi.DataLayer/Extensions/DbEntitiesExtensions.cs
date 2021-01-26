using System;
using TwitterApi.DataLayer.Entities.Models;

namespace WebApi.DataLayer.Extensions
{
    public static class DbEntitiesExtensions
    {
        /// <summary>
        /// Возвращает статус просроченности токена обновления
        /// </summary>
        /// <param name="refreshToken">Токен обновления</param>
        /// <returns></returns>
        public static bool IsExpired(this Tokens refreshToken)
        {
            return refreshToken.ExpireTime <= DateTime.UtcNow;
        }
    }
}