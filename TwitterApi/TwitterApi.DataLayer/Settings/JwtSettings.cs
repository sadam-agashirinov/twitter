using Microsoft.Extensions.Configuration;

namespace TwitterApi.DataLayer.Settings
{
    /// <summary>
    /// Настройки для JWT
    /// </summary>
    public class JwtSettings
    {
        private static JwtSettings instance = null;

        public static JwtSettings Instance => instance;

        /// <summary>
        /// Значение ключа
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// Издатель токена
        /// </summary>
        public string Issuer { get; set; }
        /// <summary>
        /// Потребитель токена
        /// </summary>
        public string Audience { get; set; }
        /// <summary>
        /// Время жизни токена в минутах
        /// </summary>
        public int AccessTokenLifeTime { get; set; }
        /// <summary>
        /// Время жизни токена обновления в днях
        /// </summary>
        public int RefreshTokenLifeTime { get; set; }

        public static void Init(IConfigurationSection configurationSection)
        {
            instance = new JwtSettings();
            configurationSection.Bind(instance);
        }

        private JwtSettings()
        {

        }
    }
}