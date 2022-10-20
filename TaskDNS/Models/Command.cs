using System.ComponentModel.DataAnnotations;

namespace TaskDNS.Models
{
    /// <summary>
    /// Класс являющийся моделью для работы с базой данных
    /// </summary>
    public class Command
    {
        /// <summary>
        /// Порядковый номер в базе данных.
        /// </summary>
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// Время.
        /// </summary>
        public string Data { get; set; }
        /// <summary>
        /// Текст введенной команды от клиента.
        /// </summary>
        public string TextCommand { get; set; }
        
    }
}