using System.Text.Json.Serialization;

namespace TaskDNS.Network.Dto
{
    /// <summary>
    /// Тип данных по отправке текстовых данных консоли клиенту.
    /// </summary>
    public class ConsoleDataDto
    {
        /// <summary>
        /// Текущая директория консоли.
        /// </summary>
        [JsonPropertyName("directory")]
        public string Directory { get; set; }
        /// <summary>
        /// Текущая под директории.
        /// </summary>
        [JsonPropertyName("subdirectory")]
        public string[] SubDirectory { get; set; }
        /// <summary>
        /// История введеных команд пользователем.
        /// </summary>
        [JsonPropertyName("history")]
        public CommandDto[] History { get; set; }
    }
}