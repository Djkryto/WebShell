namespace TaskDNS.Channels.Interface
{
    /// <summary>
    /// Интерфейс для получение статуса о команде.
    /// </summary>
    public interface ICMDCommand
    { 
        /// <summary>
        /// Статус о текущей команде.
        /// </summary>
        public Status Status { get; }
    }
}
