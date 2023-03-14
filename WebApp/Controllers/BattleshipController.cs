using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class BattleshipController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Lobby()
        {
            return View();
        }
    }
}
