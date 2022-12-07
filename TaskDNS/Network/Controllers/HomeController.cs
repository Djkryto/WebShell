using Microsoft.AspNetCore.Mvc;

namespace TaskDNS.Network.Controllers
{
    /// <summary>
    /// Класс отображающий страницы.
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// Главная страница.
        /// </summary>
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}