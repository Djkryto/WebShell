namespace TaskDNS.Models.Dto
{
    /// <summary>
    /// DTO отправка истории команд клиенту.
    /// </summary>
    public class CommandDto
    {
        /// <summary>
        /// Текст комманды.
        /// </summary>
        public string TextCommand { get;}

        /// <summary>
        /// .ctor
        /// </summary>
        public CommandDto(Command command)
        {
            TextCommand = command.TextCommand;
        }
    }
}
