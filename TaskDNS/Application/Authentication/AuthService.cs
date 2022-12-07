using TaskDNS.Application.Authentication.Encryption;
using TaskDNS.Authentication.Local_Storage;
using TaskDNS.Domain.Interface;
using TaskDNS.Database.Model;

namespace TaskDNS.Application.Authentication
{
    /// <summary>
    /// Класс для Аутентификации и Авторизации пользователя
    /// </summary>
    public class AuthService
    {
        private readonly AuthProviderFactory authFactory;
        private readonly IUserRepository _userRepository;

        /// <summary>
        /// .ctor
        /// </summary>
        public AuthService(IConfiguration configuration, IUserRepository userRepository, IHttpContextAccessor httpContext, ITokenRepository tokenRepository)
        {
            _userRepository = userRepository;

            authFactory = new AuthProviderFactory(configuration, userRepository, httpContext, tokenRepository);
        }

        /// <summary>
        /// Регистрация пользователя в базе данных.
        /// </summary>
        public async Task<string> RegistrAsync(string login, string password)
        {
            password = ProcessingPassword(password);
            var users = await _userRepository.GetUsersAsync();

            if (users.Any(x => x.Login == login & x.Password == password))
                return string.Empty;

            await _userRepository.AddAsync(new User { Login = login, Password = password });
            await _userRepository.SaveAsync();

            return string.Empty;
        }

        /// <summary>
        /// Получение JWT токена.
        /// </summary>
        public Task<string> AuthAsync(string login, string password, bool isJWT) => authFactory.CreateInstance(isJWT).AuthenticationAsync(login, ProcessingPassword(password));

        private static string ProcessingPassword(string password) => EncryptionHandler.EncodingPassword(password);
    }
}