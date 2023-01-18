using TaskDNS.Application.Authentication.Interface;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Options;
using TaskDNS.Domain.Interface;
using System.Security.Claims;
using TaskDNS.Domain.Model;
using System.Text;

namespace TaskDNS.Authentication.Local_Storage
{
    /// <summary>
    /// Класс авторизовывающий пользователя через JWT.
    /// </summary>
    public  class JWTProvider : IAuthentication
    {
        private readonly PropertyJWT _jwtOptions;
        private readonly IUserRepository _userRepository;
        private readonly ITokenRepository _tokenRepository;

        /// <summary>
        /// .ctor
        /// </summary>
        public JWTProvider(IOptions<PropertyJWT> jwtOptions, IUserRepository userRepository,ITokenRepository tokenRepository) 
        {
            _jwtOptions = jwtOptions.Value;
            _userRepository = userRepository;    
            _tokenRepository = tokenRepository;
        }

        /// <summary>
        /// Проверка пользовательских данных при успешной проверке возвращает токен.
        /// </summary>
        public async Task<string> AuthenticationAsync(string login, string password)
        {
            var users = await _userRepository.GetUsersAsync();
            var tokens = await _tokenRepository.GetAllTokensAsync();

            if (!users.Any(x => x.Login == login & x.Password == password))
            {
                return String.Empty;
            }

            var user = users.Where(x => x.Login == login & x.Password == password).First();
            var token = CreateToken();

            if(tokens.Any(x => x.IdUser == user.Id))
            {
                await _tokenRepository.UpdateAsync(user.Id, token);
            }
            else
            {
               await _tokenRepository.AddAsync(new() { IdUser = user.Id, Value = token });
               await _tokenRepository.SaveAsync();
            }

            return token;
        }

        private string CreateToken()
        {
            var issuer = _jwtOptions.Issuer;
            var audience = _jwtOptions.Audience;
            var key = Encoding.ASCII.GetBytes(_jwtOptions.Key);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("id", Guid.NewGuid().ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(30),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var stringToken = tokenHandler.WriteToken(token);

            return stringToken;
        }
    }
}
