namespace TaskDNS.Application.Authentication.Interface
{
    /// <summary>
    /// Интерфейс явялющийся посредником между реализуемыми провайдерами токена или cookie.
    /// </summary>
    public interface IAuthentication
    {
        /// <summary>
        /// Авторизация установленного провайдера.
        /// </summary>
        /// <param name="login">Логин пользователя.</param>
        /// <param name="password">Пароль пользователя.</param>
        /// <returns></returns>
        public Task<string> AuthenticationAsync(string login, string password);
    }
}
