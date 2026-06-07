using AForge.Video.DirectShow;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using WebGitPush.ViewModels;
using FilterInfo = AForge.Video.DirectShow.FilterInfo;

namespace WebGitPush.Controllers
{
    public class MainController : Controller
    {
        private static VideoCaptureDevice videoSource;
        private static byte[] _bufImage = new byte[0];
        private static CancellationTokenSource _cancellationTokenSource;
        private static bool _isStopping = false;

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
            try
            {
                StopCameraSafely();

                _isStopping = false;
                videoSource = new VideoCaptureDevice(deviceName);
                videoSource.NewFrame += VideoSourceNewFrame;
                videoSource.Start();

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public IActionResult StopCamera()
        {
            StopCameraSafely();
            return RedirectToAction("Index");
        }

        private void StopCameraSafely()
        {
            try
            {
                _isStopping = true;
                _cancellationTokenSource?.Cancel();

                if (videoSource != null)
                {
                    try
                    {
                        if (videoSource.IsRunning)
                        {
                            videoSource.SignalToStop();

                            int waitCount = 0;
                            while (videoSource.IsRunning && waitCount < 50) // максимум 1 секунда
                            {
                                System.Threading.Thread.Sleep(20);
                                waitCount++;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error stopping: {ex.Message}");
                    }
                    finally
                    {
                        try
                        {
                            videoSource.NewFrame -= VideoSourceNewFrame;
                        }
                        catch { }

                        videoSource = null;
                    }
                }

                _bufImage = new byte[0];
            }
            catch (Exception ex)
            {
                Console.WriteLine($"StopCameraSafely error: {ex.Message}");
            }
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
            Response.Headers["Connection"] = "keep-alive";

            var ae = new ASCIIEncoding();
            _cancellationTokenSource = new CancellationTokenSource();

            try
            {
                while (videoSource != null && videoSource.IsRunning && !_cancellationTokenSource.Token.IsCancellationRequested)
                {
                    if (_bufImage != null && _bufImage.Length > 0)
                    {
                        try
                        {
                            var boundary = ae.GetBytes($"\r\n--myboundary\r\nContent-Type: image/jpeg\r\nContent-Length: {_bufImage.Length}\r\n\r\n");
                            await Response.Body.WriteAsync(boundary, 0, boundary.Length, _cancellationTokenSource.Token);
                            await Response.Body.WriteAsync(_bufImage, 0, _bufImage.Length, _cancellationTokenSource.Token);
                            await Response.Body.FlushAsync(_cancellationTokenSource.Token);
                        }
                        catch (OperationCanceledException)
                        {
                            break;
                        }
                        catch (Exception)
                        {
                            break;
                        }
                    }
                    await Task.Delay(50, _cancellationTokenSource.Token);
                }
            }
            catch (OperationCanceledException)
            {
            }
            catch (Exception)
            {
            }
        }

        private void VideoSourceNewFrame(object sender, AForge.Video.NewFrameEventArgs eventArgs)
        {
            if (_isStopping) return;

            try
            {
                using (var ms = new MemoryStream())
                {
                    eventArgs.Frame.Save(ms, ImageFormat.Jpeg);
                    var newImage = ms.ToArray();
                    Interlocked.Exchange(ref _bufImage, newImage);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Frame error: {ex.Message}");
            }
        }
    }
}