using System.Threading.Channels;
using TaskDNS.Channels.Interface;

namespace TaskDNS.Channels
{
    /// <summary>
    /// Канал данных.
    /// </summary>
    public static class ChannelProvider
    {
        public static Channel<ICMDCommand> CommandChannel = Channel.CreateUnbounded<ICMDCommand>();
    }
}
