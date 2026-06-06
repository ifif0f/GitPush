using AForge.Video.DirectShow;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using FilterInfo = AForge.Video.DirectShow.FilterInfo;

namespace WebGitPush.Controllers
{
    public class MainController : Controller
    {
        private static VideoCaptureDevice videoSource;
        private static byte[] _bufImage = new byte[0];

        public IActionResult Index()
        {
            try
            {
                var videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
                ViewBag.DeviceList = videoDevices.Cast<FilterInfo>()
                    .Select(x => new SelectListItem { Text = x.Name, Value = x.MonikerString }).ToList();
            }
            catch
            {
                ViewBag.DeviceList = new List<SelectListItem>();
            }

            ViewBag.IsRunning = videoSource != null && videoSource.IsRunning;
            return View();
        }

        [HttpPost]
        public IActionResult StartCamera(string deviceName)
        {
            if (videoSource != null && videoSource.IsRunning)
            {
                videoSource.Stop();
                videoSource = null;
            }

            videoSource = new VideoCaptureDevice(deviceName);
            videoSource.NewFrame += VideoSourceNewFrame;
            videoSource.Start();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult StopCamera()
        {
            if (videoSource != null && videoSource.IsRunning)
            {
                videoSource.Stop();
                videoSource = null;
            }
            _bufImage = new byte[0];
            return RedirectToAction("Index");
        }

        public async Task Video()
        {
            if (videoSource == null || !videoSource.IsRunning)
            {
                Response.StatusCode = 404;
                return;
            }

            Response.ContentType = "multipart/x-mixed-replace; boundary=--myboundary";
            Response.Headers["Cache-Control"] = "no-cache";

            var ae = new ASCIIEncoding();

            try
            {
                while (videoSource != null && videoSource.IsRunning)
                {
                    if (_bufImage.Length > 0)
                    {
                        var boundary = ae.GetBytes($"\r\n--myboundary\r\nContent-Type: image/jpeg\r\nContent-Length: {_bufImage.Length}\r\n\r\n");
                        await Response.Body.WriteAsync(boundary, 0, boundary.Length);
                        await Response.Body.WriteAsync(_bufImage, 0, _bufImage.Length);
                        await Response.Body.FlushAsync();
                    }
                    await Task.Delay(50);
                }
            }
            catch (Exception)
            {
                //если клиент оффнулся
            }
        }

        private void VideoSourceNewFrame(object sender, AForge.Video.NewFrameEventArgs eventArgs)
        {
            using (var ms = new MemoryStream())
            {
                eventArgs.Frame.Save(ms, ImageFormat.Jpeg);
                _bufImage = ms.ToArray();
            }
        }
    }
}