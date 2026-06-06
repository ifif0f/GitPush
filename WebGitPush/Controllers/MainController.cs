using Microsoft.AspNetCore.Mvc;
using WebGitPush.ViewModels;

namespace WebGitPush.Controllers
{
    public class MainController : Controller
    {
        public async Task<IActionResult> Index(int parkingBuildingId = 0, int storageBuildingId = 0)
        {
            var vm = new VMIndex();
            await vm.LoadDataAsync(parkingBuildingId, storageBuildingId);

            System.Diagnostics.Debug.WriteLine($"Новостей загружено: {vm.NewsItems.Count}");
            foreach (var news in vm.NewsItems)
            {
                System.Diagnostics.Debug.WriteLine($"Новость: {news.title}, изображений: {news.images?.Count ?? 0}");
            }

            return View(vm);
        }
    }
}