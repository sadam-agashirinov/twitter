using System;

namespace TwitterApi.DataLayer.Entities
{
    /// <summary>
    /// Информация об авторизованном пользователе
    /// </summary>
    public class AuthenticatedUserInfo
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// ФИО
        /// </summary>
        public string Fio { get; set; }
        /// <summary>
        /// Идентификатор филиала в котором работает сотрудник
        /// </summary>
        public Guid OfficeId { get; set; }
        /// <summary>
        /// Идентификатор должности сотрудника
        /// </summary>
        public int JobPositionId { get; set; }
        /// <summary>
        /// Мнемоника МФЦ
        /// </summary>
        public string OfficeMnemo { get; set; }
    }
}