using TaskDNS.Channels.Interface;

namespace TaskDNS.Channels.Models
{
    /// <summary>
    /// Класс со статусом выполненой команды с ошибкой.
    /// </summary>
    public class Executive : ICMDCommand
    {
        /// <summary>
        /// Статус в данном классе имеет значение Executive. 
        /// </summary>
        public Status Status { get; }
        /// <summary>
        /// Ответ с сервера.
        /// </summary>
        public string Output { get; }
        /// <summary>
        /// Определяет значение свойств Output и Status.
        /// </summary>
        /// <param name="output">Ответ с сервера.</param>
        public Executive(string output)
        {
            Status = Status.Executive;
            Output = output;
        }
    }
}
