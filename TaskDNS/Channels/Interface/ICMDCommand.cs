namespace TaskDNS.Channels.Interface
{
    public interface ICMDCommand
    {
        public Status Status { get; }
    }

    public enum Status
    {
        Executive = 0,
        Error = 1,
        Complete = 2
    }
}
