using TaskDNS.Application.Authentication.Interface;
using TaskDNS.Domain.Interface;

namespace TaskDNS.Authentication.Local_Storage
{
    /// <summary>
    /// Класс предоставляющий поставщика аудентификации и авторищации.
    /// </summary>
    public class AuthProviderFactory
    {
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;
        private readonly ITokenRepository _tokenRepository;
        private readonly IHttpContextAccessor _httpContext;

        /// <summary>
        /// .ctor
        /// </summary>
        public AuthProviderFactory(IConfiguration configuration, IUserRepository userRepository, IHttpContextAccessor httpContext,ITokenRepository tokenRepository) 
        {
            _tokenRepository = tokenRepository;
            _userRepository = userRepository;
            _configuration = configuration;
            _httpContext = httpContext;
        }

        /// <summary>
        /// Метод возвращающий поставщика в виде интерфейса.
        /// </summary>
        /// <param name="isJWT">Выбор поставщика.</param>
        /// <returns></returns>
        public IAuthentication CreateInstance(bool isJWT)
        {
            if (isJWT)
                return new JWTProvider(_configuration,_userRepository,_tokenRepository);

            return new CookieProvider(_httpContext, _userRepository); 
        }
    }
}
