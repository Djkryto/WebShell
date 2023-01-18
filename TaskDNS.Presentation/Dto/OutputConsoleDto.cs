using TaskDNS.Application.Enum;

namespace TaskDNS.Network.Dto
{
    /// <summary>
    /// Dto для отправки данных консоли на клиент.
    /// </summary>
    public class OutputConsoleDto
    {
        /// <summary>
        /// Строка полученная от консоли.
        /// </summary>
        public string Output { get; set; }

        /// <summary>
        /// Статус с вывода.
        /// </summary>
        public CommandStatus Status { get; set; }

        /// <summary>
        /// .ctor
        /// </summary>
        public OutputConsoleDto(string line, CommandStatus status)
        {
            Output = line;
            Status = status;
        }
    }
}
