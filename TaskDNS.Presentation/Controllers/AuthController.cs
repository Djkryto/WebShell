using Microsoft.AspNetCore.Authorization;
using TaskDNS.Application.Authentication;
using Microsoft.AspNetCore.Mvc;
using TaskDNS.Database.Model;
using TaskDNS.Network.Dto;

namespace TaskDNS.Network.Controllers
{
    /// <summary>
    /// Контроллер аудентификации и авторизации.
    /// </summary>
    [ApiController]
    [Route("auth")]
    public class AuthController : Controller
    {
        private readonly AuthService _authService;

        /// <summary>
        /// .ctor
        /// </summary>
        public AuthController(AuthService authService) => _authService = authService;

        /// <summary>
        /// Авторизация.
        /// </summary>
        [HttpPost]
        [AllowAnonymous]
        [Route("login")]
        public async Task<IActionResult> LoginAsync([FromBody] ClientDataDto clientData) => Ok(await _authService.AuthAsync(clientData.User.Login, clientData.User.Password, clientData.IsJWT));

        /// <summary>
        /// Регистрация пользователя.
        /// </summary>
        [HttpPost]
        [AllowAnonymous]
        [Route("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] User model) => Ok(await _authService.RegistrAsync(model.Login, model.Password));
    }
}