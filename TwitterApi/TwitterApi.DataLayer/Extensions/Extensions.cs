using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using TwitterApi.DataLayer.Entities;
using TwitterApi.DataLayer.Utils;

namespace TwitterApi.DataLayer.Extensions
{
    public static class Extensions
    {
        public static AuthenticatedUserInfo GetAuthenticatedUserInfo(this HttpContext httpContext)
        {
            var token = httpContext.GetAccessToken();

            if (!ValidationUtils.IsValidName(token)) throw new Exception("Ошибка получения токена доступа.");

            return JwtTokenUtils.CreateAuthenticatedUserInfo(token);
        }

        public static string GetAccessToken(this HttpContext httpContext)
        {
            return httpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        }
    }
}