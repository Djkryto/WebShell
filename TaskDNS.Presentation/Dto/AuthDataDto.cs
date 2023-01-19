using System.Text.Json.Serialization;
using TaskDNS.Database.Model;

namespace TaskDNS.Network.Dto
{
    /// <summary>
    /// Тип данных полученных от клиента.
    /// </summary>
    public class AuthDataDto
    {
        /// <summary>
        /// Логин пользователя.
        /// </summary>
        [JsonPropertyName("login")]
        public string Login { get; init; }
        /// <summary>
        /// Пароль пользователя.
        /// </summary>
        [JsonPropertyName("password")]
        public string Password { get; init; }
        /// <summary>
        /// Тип токена.
        /// </summary>
        [JsonPropertyName("isjwt")]
        public bool IsJWT { get; init; }
        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="login">Логин пользователя.</param>
        /// <param name="password">Пароль пользователя.</param>
        /// <param name="isJWT">Тип токена.</param>
        public AuthDataDto(string login, string password, bool isJWT)
        {
            Login = login;
            Password = password;
            IsJWT = isJWT;
        }
    }
}