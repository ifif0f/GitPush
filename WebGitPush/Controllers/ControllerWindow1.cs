using Microsoft.AspNetCore.Mvc;

namespace WebGitPush.Controllers
{
    public class ControllerWindow1 : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}