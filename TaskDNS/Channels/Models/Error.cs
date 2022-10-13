using TaskDNS.Channels.Interface;
using TaskDNS.Controllers.Interface;

namespace TaskDNS.Channels.Models
{
    public class Error : ICMDCommand
    {
        public Status Status { get;}

        public string Output { get; }

        public Error(string output)
        {
            Status = Status.Error;
            Output = output;
        }
    }
}
