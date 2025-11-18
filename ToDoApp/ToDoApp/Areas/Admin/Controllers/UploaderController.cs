using Microsoft.AspNetCore.Mvc;
using ToDoApp.Areas.Admin.Controllers;

namespace ToDoApp.Controllers
{
    public class UploaderController : AdminBaseController
    {
        private readonly IWebHostEnvironment _env;

        public UploaderController(IWebHostEnvironment env)
        =>_env = env;

        [HttpPost]
        public async Task<IActionResult> UploadImage(IFormFile upload)
        {
            if (upload != null && upload.Length > 0)
            {
                var fileName = Path.GetFileName(upload.FileName);
                var uploadsFolder = Path.Combine(_env.WebRootPath, "content/images/CkEditor");

                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await upload.CopyToAsync(fileStream);
                }

                var url = Url.Content($"~/content/images/CkEditor/{fileName}");
                return Json(new { uploaded = true, url });
            }

            return Json(new { uploaded = false, error = new { message = "Upload failed" } });
        }
    }
}
