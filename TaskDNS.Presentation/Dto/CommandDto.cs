using System.Text.Json.Serialization;
using TaskDNS.Database.Model;

namespace TaskDNS.Network.Dto
{
    /// <summary>
    /// DTO отправка истории команд клиенту.
    /// </summary>
    public class CommandDto
    {
        /// <summary>
        /// Текст комманды.
        /// </summary>
        [JsonPropertyName("textcommand")]
        public string TextCommand { get; }

        /// <summary>
        /// .ctor
        /// </summary>
        public CommandDto(Command command)
        {
            TextCommand = command.TextCommand;
        }
    }
}