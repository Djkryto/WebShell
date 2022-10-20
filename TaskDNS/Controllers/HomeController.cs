using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TaskDNS.Models;

namespace TaskDNS.Controllers
{
    /// <summary>
    /// Класс обрабатывающий отображения страниц сайта.
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// Главная страница.
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return View();
        }
    }
}