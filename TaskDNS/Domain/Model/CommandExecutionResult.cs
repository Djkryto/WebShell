using TaskDNS.Application.Enum;

namespace TaskDNS.Domain.Models
{
    /// <summary>
    /// Класс со статусами выполнения команд.
    /// </summary>
    public class CommandExecutionResult
    {
        /// <summary>
        /// Статус в данном классе имеет значение Error. 
        /// </summary>
        public CommandStatus Status { get; private set; }

        /// <summary>
        /// Ответ с сервера. 
        /// </summary>
        public string Output { get; private set; }

        /// <summary>
        /// Индекс пользователя.
        /// </summary>
        public string ConnectionId { get; private set; }

        /// <summary>
        /// .ctor
        /// </summary>
        private CommandExecutionResult() { }

        /// <summary>
        /// .ctor со статусом выполненой команды.
        /// </summary>
        public static CommandExecutionResult Success(string output, string connectionId)
        {
            return new CommandExecutionResult
            {
                Status = CommandStatus.Complete,
                Output = output,
                ConnectionId = connectionId
            };
        }

        /// <summary>
        /// .ctor со статусом ошибки при выполнения команды.
        /// </summary>
        public static CommandExecutionResult Error(string output, string connectionId)
        {
            return new CommandExecutionResult
            {
                Status = CommandStatus.Error,
                Output = output,
                ConnectionId = connectionId
            };
        }

        /// <summary>
        /// .ctor со статусом выполнения команды.
        /// </summary>
        public static CommandExecutionResult Executive(string output, string connectionId)
        {
            return new CommandExecutionResult
            {
                Status = CommandStatus.Executive,
                Output = output,
                ConnectionId = connectionId
            };
        }
    }
}
