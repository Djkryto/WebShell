using TaskDNS.Channels.Interface;

namespace TaskDNS.Channels.Models
{
    public class Executive : ICMDCommand
    {
        public Status Status { get; }
        public string Output { get; }
        public Executive(string output)
        {
            Status = Status.Executive;
            Output = output;
        }
    }
}
