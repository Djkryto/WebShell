namespace TaskDNS.Application.Enum
{
    /// <summary>
    /// Состояние о статусе текущей команды.
    /// </summary>
    public enum CommandStatus : byte
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