namespace TaskDNS.Application.Model
{
    /// <summary>
    /// Свойство токена при запуске приложения.
    /// </summary>
    public class PropertyJWT
    {
        /// <summary>
        /// Ключ токена.
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// Издатель токена.
        /// </summary>
        public string Issuer { get; set; }
        /// <summary>
        /// Принимающие токен.
        /// </summary>
        public string Audience { get; set; }
    }
}