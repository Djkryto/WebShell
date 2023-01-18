using TaskDNS.Application.Authentication.Interface;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;
using TaskDNS.Application.Model;
using TaskDNS.Domain.Interface;

namespace TaskDNS.Authentication.Local_Storage
{
    /// <summary>
    /// Класс предоставляющий поставщика аудентификации и авторищации.
    /// </summary>
    public class AuthProviderFactory
    {
        private readonly IOptions<PropertyJWT> _jwtOptions;
        private readonly IUserRepository _userRepository;
        private readonly ITokenRepository _tokenRepository;
        private readonly IHttpContextAccessor _httpContext;

        /// <summary>
        /// .ctor
        /// </summary>
        public AuthProviderFactory(IOptions<PropertyJWT> jwtOptions, IUserRepository userRepository, IHttpContextAccessor httpContext,ITokenRepository tokenRepository) 
        {
            _tokenRepository = tokenRepository;
            _userRepository = userRepository;
            _jwtOptions = jwtOptions;
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
                return new JWTProvider(_jwtOptions, _userRepository,_tokenRepository);

            return new CookieProvider(_httpContext, _userRepository); 
        }
    }
}