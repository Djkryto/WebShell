using TaskDNS.Channels.Interface;

namespace TaskDNS.Channels.Models
{
    /// <summary>
    /// Класс со статусом ошибка при выполнении команды.
    /// </summary>
    public class Executive : ICommandWithStatus
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
        /// .ctor
        /// </summary>
        public Executive(string output)
        {
            Status = Status.Executive;
            Output = output;
        }
    }
}
