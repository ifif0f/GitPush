using Microsoft.AspNetCore.Mvc;
using WebGitPush.ViewModels;

namespace WebGitPush.Controllers
{
    public class Window1Controller : Controller
    {
        public async Task<IActionResult> Index(int parkingBuildingId = 0, int storageBuildingId = 0)
        {
            var vm = new VMIndex();
            await vm.LoadDataAsync(parkingBuildingId, storageBuildingId);

            return View(vm);
        }
    }
}