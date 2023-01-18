using TaskDNS.Application.Authentication.Interface;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using TaskDNS.Domain.Interface;
using System.Security.Claims;
using TaskDNS.Database.Model;

namespace TaskDNS.Authentication.Local_Storage
{
    /// <summary>
    /// Класс авторизовывающий пользователя черезе Cookie.
    /// </summary>
    public class CookieProvider : IAuthentication
    {
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContext;

        /// <summary>
        /// .ctor
        /// </summary>
        public CookieProvider(IHttpContextAccessor httpContext, IUserRepository userRepository)
        {
            _httpContext = httpContext;
            _userRepository = userRepository;
        }

        /// <summary>
        /// Авторизация пользователя на сервере с возвращаемым статусом обработки.
        /// </summary>
        public async Task<string> AuthenticationAsync(string login, string password)
        {
            var users = await _userRepository.GetUsersAsync();

            if (!users.Any(x => x.Login == login & x.Password == password))
                return String.Empty;

            var user = users.Where(x => x.Login == login & x.Password == password).First();

            if (user == null)
                return String.Empty;

            await SingIn(user);

            return String.Empty;
        }

        private async Task SingIn(User user)
        {
            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);
            identity.AddClaim(new Claim(ClaimTypes.Email, user.Login));
            identity.AddClaim(new Claim(ClaimTypes.Name, user.Login));
            var principal = new ClaimsPrincipal(identity);

            await _httpContext.HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(principal), new AuthenticationProperties
                {
                    AllowRefresh = true,
                    ExpiresUtc = DateTimeOffset.Now.AddDays(1),
                    IsPersistent = true
                });
        }
    }
}