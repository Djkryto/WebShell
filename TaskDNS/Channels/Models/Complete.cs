using TaskDNS.Channels.Interface;
using TaskDNS.Controllers.Interface;

namespace TaskDNS.Channels.Models
{
    /// <summary>
    /// Класс со статусом выполненой команды с правильным завершением.
    /// </summary>
    public class Complete : ICommandWithStatus
    {
        /// <summary>
        /// В данном классе имеет значение Complete.
        /// </summary>
        public Status Status { get; }

        /// <summary>
        /// .ctor
        /// </summary>
        public Complete()
        {
           Status = Status.Complete;
        }
    }
}