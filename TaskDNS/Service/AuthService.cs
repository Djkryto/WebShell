using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using TaskDNS.Controllers.Interface;
using TaskDNS.Models;
using TaskDNS.Models.SQLServer;
using TaskDNS.Models.SQLServer.Repository;

namespace TaskDNS.Service
{
    public class AuthService
    {
        private TokenValidationParameters TokenParameters { get; init; }
        private readonly IConfiguration _configuration;
        private IUserRepository _userRepository;

        public AuthService(IConfiguration configuration, SQLContext sqlContext)
        {
            _configuration = configuration;
            _userRepository = new UserRepository(sqlContext);

            TokenParameters = new TokenValidationParameters()
            {
                ValidIssuer = _configuration["Issuer"],
                ValidAudience = _configuration["Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["JwtKey"]))
            };
        }

        public string Auth(string login, string password)
        {
            var user = _userRepository.GetUsers().Where(x => x.Login == login & x.Password == password).First();

            if (user == null)
                return String.Empty;

            var token = CreateToken();
            _userRepository.WriteToken(user.Id, token);

            return token;
        }

        public byte Registration(string login, string password)
        {
            try
            {
                _userRepository.Add(new User { Login = login, Password = password, Token = "" });
                _userRepository.Save();
                return 0;
            }
            catch
            {
                return 1;
            }
        }

        public string CreateToken()
        {
            var issuer = _configuration["Issuer"];
            var audience = _configuration["Audience"];
            var key = Encoding.ASCII.GetBytes(_configuration["JwtKey"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("id", Guid.NewGuid().ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(5),
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
