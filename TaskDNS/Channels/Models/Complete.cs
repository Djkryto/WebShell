using TaskDNS.Channels.Interface;
using TaskDNS.Controllers.Interface;

namespace TaskDNS.Channels.Models
{
    public class Complete : ICMDCommand
    {
        public Status Status { get; }

        public Complete()
        {
           Status = Status.Complete;
        }
    }
}