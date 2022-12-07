using System.ComponentModel.DataAnnotations;

namespace TaskDNS.Database.Model
{
    /// <summary>
    /// Класс являющийся моделью истории командя в базе данных.
    /// </summary>
    public class Command
    {
        /// <summary>
        /// Порядковый номер в базе данных.
        /// </summary>
        [Key]
        public int Id { get; init; }

        /// <summary>
        /// Порядковый номер в общем узле.
        /// </summary>
        public string IdUserInHub { get; init; }

        /// <summary>
        /// Текст введенной команды от клиента.
        /// </summary>
        public string TextCommand { get; init; }
    }
}