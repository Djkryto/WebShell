using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TaskDNS.Models;

namespace TaskDNS.Controllers
{
    /// <summary>
    /// Класс отображающий страницы.
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// Главная страница.
        /// </summary>
        public IActionResult Index()
        {
            return View();
        }
    }
}