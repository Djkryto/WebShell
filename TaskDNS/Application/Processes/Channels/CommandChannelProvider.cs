using System.Threading.Channels;
using TaskDNS.Domain.Models;

namespace TaskDNS.Application.Processes.Channels
{
    /// <summary>
    /// Класс предоставляющий канал данных.
    /// </summary>
    public static class CommandChannelProvider
    {
        /// <summary>
        /// Канал данных.
        /// </summary>
        public static Channel<CommandExecutionResult> CommandResultChannel = Channel.CreateUnbounded<CommandExecutionResult>();
    }
}
