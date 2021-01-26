using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using TwitterApi.DataLayer.Common;
using TwitterApi.DataLayer.Entities;
using TwitterApi.DataLayer.Entities.Models;
using TwitterApi.DataLayer.Settings;

namespace TwitterApi.DataLayer.Utils
{
    /// <summary>
    /// Итилита для Jwt
    /// </summary>
    public class JwtTokenUtils
    {
        /// <summary>
        /// Генерация токена
        /// </summary>
        /// <param name="claimsPrincipal"></param>
        /// <returns></returns>
        public static string GenerateAccessToken(ClaimsPrincipal claimsPrincipal)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(JwtSettings.Instance.Key);
                var tokenDescription = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claimsPrincipal.Claims),
                    Expires = DateTime.UtcNow.AddMinutes(JwtSettings.Instance.AccessTokenLifeTime),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                        SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescription);
                return tokenHandler.WriteToken(token);
            }
            catch (Exception e)
            {
                WebApiLogger.LogException(e);
                return string.Empty;
            }
        }

        public static ClaimsPrincipal CreateClaimsPrincipal(Users user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
            };

            return new ClaimsPrincipal(new ClaimsIdentity(claims));
        }

        /// <summary>
        /// Создать объект хранящий информацию об авторизованном пользователе
        /// </summary>
        /// <param name="token">Токен</param>
        /// <returns></returns>
        public static AuthenticatedUserInfo CreateAuthenticatedUserInfo(string token)
        {
            var principal = GetClaimsPrincipal(token);

            var claims = principal.Claims.ToList();

            var id = Guid.Parse(claims.First(x => x.Type == JwtRegisteredClaimNames.NameId).Value);
            var userName = claims.First(x => x.Type == JwtRegisteredClaimNames.UniqueName).Value;

            return new AuthenticatedUserInfo
            {
                Id = id,
                UserName = userName
            };
        }

        /// <summary>
        /// Генерация рефреш токена
        /// </summary>
        /// <returns>Рефреш токен</returns>
        public static string GenerateRefreshToken()
        {
            using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            var randomNumber = new byte[64];
            rngCryptoServiceProvider.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        /// <summary>
        /// Получение разрешений из токена
        /// </summary>
        /// <param name="token">Токен</param>
        /// <returns></returns>
        public static ClaimsPrincipal GetClaimsPrincipal(string token)
        {
            var securityToken = new JwtSecurityToken(token);
            var claims = securityToken.Claims;

            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claims));

            return claimsPrincipal;
        }

        /// <summary>
        /// Проверка валидности токена
        /// </summary>
        /// <param name="token"></param>
        public static bool ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                var tokenValidationParameters = CreateTokenValidationParameters();
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken validatedToken);
            }
            catch (SecurityTokenExpiredException)
            {
                //ignore
            }
            catch (Exception e)
            {
                WebApiLogger.LogException(e);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Параметры валидации токена
        /// </summary>
        /// <returns></returns>
        public static TokenValidationParameters CreateTokenValidationParameters()
        {
            var key = Encoding.ASCII.GetBytes(JwtSettings.Instance.Key);
            return new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            };
        }

        private class ClaimNamesType
        {
            public const string OfficeId = "OfficeId";
            public const string OfficeMnemo = "OfficeMnemo";
            public const string JobPositionId = "JobPositionId";
        }
    }
}