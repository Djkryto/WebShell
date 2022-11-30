using Microsoft.AspNetCore.Mvc;
using TaskDNS.Models;
using TaskDNS.Service;

namespace TaskDNS.Controllers
{
    [Route("jwt")]
    public class JwtController : Controller
    {
        private AuthService _authService;

        public JwtController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        [Route("login")]
        public string Login([FromBody]User model)
        {
            return _authService.Auth(model.Login, model.Password);
        }

        [HttpPost]
        [Route("register")]
        public byte Register([FromBody] User model)
        {
            return _authService.Registration(model.Login, model.Password);
        }

    }
}
