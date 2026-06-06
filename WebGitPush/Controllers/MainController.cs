using Microsoft.AspNetCore.Mvc;

namespace WebGitPush.Controllers
{
    public class MainController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}