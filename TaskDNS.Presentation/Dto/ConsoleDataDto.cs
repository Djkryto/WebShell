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
        public string Directory { get; set; }
        /// <summary>
        /// Текущая под директории.
        /// </summary>
        public string[] SubDirectory { get; set; }
        /// <summary>
        /// История введеных команд пользователем.
        /// </summary>
        public CommandDto[] History { get; set; }
    }
}