using Microsoft.AspNetCore.Mvc;

namespace WebGitPush.Controllers
{
    public class HomeController : Controller
    {
        public RedirectResult Index()
        {
            return Redirect("/Main");
        }
    }
}
