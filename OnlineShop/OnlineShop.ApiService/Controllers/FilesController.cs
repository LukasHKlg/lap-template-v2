using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace OnlineShop.ApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;
        private readonly ILogger _logger;
        public FilesController(IWebHostEnvironment env, ILogger<FilesController> logger)
        {
            _env = env;
            _logger = logger;
        }

        [HttpPost("uploadProductPicture")]
        [Authorize(Roles = "Admin")]
        [Consumes("multipart/form-data")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> UploadProductPicture([FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded");

            var upload = Path.Combine(_env.WebRootPath, "uploads/products");
            if(!Directory.Exists(upload)) Directory.CreateDirectory(upload);

            var safeFileName = Path.GetFileName(file.FileName);
            var filePath = Path.Combine(upload, safeFileName);

            using(var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var fileUrl = $"/uploads/products{safeFileName}";
            return Ok(new { FileUrl = fileUrl });
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteProductPicture(int id)
        {
            try
            {
                var filePath = Path.Combine(_env.WebRootPath, $"uploads/products/{id}.jpg");

                if(!System.IO.File.Exists(filePath)) return NoContent();

                System.IO.File.Delete(filePath);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error while deleting product picture. Error: " + ex.ToString());
                return BadRequest("Picture could not be deleted.");
                throw;
            }
        }
    }
}
