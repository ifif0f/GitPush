using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebGitPush.Models
{
    public class ImageModel
    {
        public bool IsRunning { get; set; }

        public List<SelectListItem> DeviceList { get; set; }
    }
}
