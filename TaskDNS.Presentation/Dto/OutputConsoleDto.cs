using System.Text.Json.Serialization;
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
        [JsonPropertyName("output")]
        public string Output { get; set; }

        /// <summary>
        /// Статус с вывода.
        /// </summary>
        [JsonPropertyName("status")]
        public byte Status { get; set; }

        /// <summary>
        /// .ctor
        /// </summary>
        public OutputConsoleDto(string line, byte status)
        {
            Output = line;
            Status = status;
        }
    }
}