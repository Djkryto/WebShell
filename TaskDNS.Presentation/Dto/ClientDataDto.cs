using TaskDNS.Database.Model;

namespace TaskDNS.Network.Dto
{
    /// <summary>
    /// Тип данных полученных от клиента.
    /// </summary>
    public class ClientDataDto
    {
        /// <summary>
        /// Логин пользователя.
        /// </summary>
        public string Login { get; init; }
        /// <summary>
        /// Пароль пользователя.
        /// </summary>
        public string Password { get; init; }
        /// <summary>
        /// Тип токена.
        /// </summary>
        public bool IsJWT { get; init; }
        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="login">Логин пользователя.</param>
        /// <param name="password">Пароль пользователя.</param>
        /// <param name="isJWT">Тип токена.</param>
        public ClientDataDto(string login, string password, bool isJWT)
        {
            Login = login;
            Password = password;
            IsJWT = isJWT;
        }
    }
}