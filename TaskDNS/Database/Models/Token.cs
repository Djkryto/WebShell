using System.ComponentModel.DataAnnotations;

namespace TaskDNS.Database.Model
{
    /// <summary>
    /// Модель для работы с таблицей токенов.
    /// </summary>
    public class Token
    {
        /// <summary>
        /// Индекс в таблице.
        /// </summary>
        [Key]
        public int Id { get; init; }
        /// <summary>
        /// Индекс пользвателя принадлежащему токен.
        /// </summary>
        public int IdUser { get; init; }
        /// <summary>
        /// Значение токена.
        /// </summary>
        public string Value { get; set; }
    }
}
