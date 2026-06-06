using Microsoft.AspNetCore.Mvc;

namespace WebGitPush.Controllers
{
    public class Window1Controller : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult List()
        {
            return View();
        }
    }
}