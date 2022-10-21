namespace TaskDNS.Channels.Interface
{
    /// <summary>
    /// Состояние о статусе текущей команды.
    /// </summary>
    public enum Status
    {
        /// <summary>
        /// Состояние выполнение команды.
        /// </summary>
        Executive = 0,

        /// <summary>
        /// Состояние ошибки при выполнении команды.
        /// </summary>
        Error = 1,

        /// <summary>
        /// Состояние правильно завершенной команды.
        /// </summary>
        Complete = 2
    }
}
