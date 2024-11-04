// Controllers/HomeController.cs
using ImageGalleryApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace ImageGalleryApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly string _uploadPath;

        public HomeController(IWebHostEnvironment env)
        {
            _uploadPath = Path.Combine(env.WebRootPath, "uploads");
            if (!Directory.Exists(_uploadPath))
            {
                Directory.CreateDirectory(_uploadPath);
            }
        }

        public IActionResult Index()
        {
            var images = Directory.GetFiles(_uploadPath)
                                  .Select(file => new ImageModel
                                  {
                                      FileName = Path.GetFileName(file),
                                      Description = "Sample description" // You can customize this as needed
                                  }).ToList();
            return View(images);
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                var filePath = Path.Combine(_uploadPath, file.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
            }
            return RedirectToAction("Index");
        }
    }
}
