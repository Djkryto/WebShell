using System.Threading.Channels;
using TaskDNS.Channels.Interface;

namespace TaskDNS.Channels
{
    public static class ChannelProvider
    {
        public static Channel<ICMDCommand> CommandChannel = Channel.CreateUnbounded<ICMDCommand>();
    }
}
