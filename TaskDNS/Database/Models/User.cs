using System.ComponentModel.DataAnnotations;

namespace TaskDNS.Database.Model
{
    /// <summary>
    /// Класс являющийся моделью пользователей в базе данных.
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