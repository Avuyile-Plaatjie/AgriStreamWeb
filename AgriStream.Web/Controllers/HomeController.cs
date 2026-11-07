using AgriStream.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AgriStream.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly BlobStorageService _blobStorageService;
        private readonly AgriStreamApiService _apiService;

        public HomeController(BlobStorageService blobStorageService, AgriStreamApiService apiService)
        {
            _blobStorageService = blobStorageService;
            _apiService = apiService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                ViewBag.Telemetry = await _apiService.GetTelemetryAsync("field1");
            }
            catch (Exception)
            {
                ViewBag.Telemetry = "Unable to connect to telemetry service.";
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadDroneImage(IFormFile file)
        {
            try
            {
                ViewBag.Telemetry = await _apiService.GetTelemetryAsync("field1");
            }
            catch (Exception)
            {
                ViewBag.Telemetry = "Unable to connect to telemetry service.";
            }

            if (file != null && file.Length > 0)
            {
                string fileUri = await _blobStorageService.UploadFileAsync(file, "drone-images");
                ViewBag.Message = $"File uploaded successfully! URI: {fileUri}";
            }
            else
            {
                ViewBag.Message = "Please select a file to upload.";
            }
            return View("Index");
        }

        [HttpPost]
        public async Task<IActionResult> StartIrrigation()
        {
            var command = new { fieldId = "field1", action = "StartWatering", durationMinutes = 30 };
            await _apiService.TriggerIrrigationAsync(command);
            TempData["IrrigationMessage"] = "Irrigation command successfully triggered and queued!";
            return RedirectToAction("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}