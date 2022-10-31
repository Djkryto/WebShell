using System.Threading.Channels;
using TaskDNS.Channels.Interface;

namespace TaskDNS.Channels
{
    /// <summary>
    /// Класс предоставляющий канал данных.
    /// </summary>
    public static class ChannelProvider
    {
        /// <summary>
        /// Канал данных.
        /// </summary>
        public static Channel<ICommandWithStatus> CommandChannel = Channel.CreateUnbounded<ICommandWithStatus>();
    }
}
