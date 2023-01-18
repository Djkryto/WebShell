using System.ComponentModel.DataAnnotations;

namespace TaskDNS.Database.Model
{
    /// <summary>
    /// Тип данных пользователя.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Порядковый номер в базе данных.
        /// </summary>
        [Key]
        public int Id { get; init; }
        /// <summary>
        /// Логин пользователя.
        /// </summary>
        public string Login { get; init; }
        /// <summary>
        /// Пароль пользователя.
        /// </summary>
        public string Password { get; init; }
    }
}