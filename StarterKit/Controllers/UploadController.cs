using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StarterKit.Data;
using StarterKit.Models;
using static StarterKit.Enums.Enum;

namespace StarterKit.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UploadController(AppDbContext context) : Controller
    {
        private readonly AppDbContext _context = context;

        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            string[] allowedExtensions = [".jpg", ".jpeg", ".png", ".gif", ".pdf"];
            string extension = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(extension))
            {
                return BadRequest("Invalid file type. Allowed types are: " + string.Join(", ", allowedExtensions));
            }

            string uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            if (!Directory.Exists(uploads))
                Directory.CreateDirectory(uploads);

            string randomFileName = Path.GetRandomFileName() + extension;

            string filePath = Path.Combine(uploads, randomFileName);
            using (FileStream stream = new(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            _context.UploadedFiles.Add(new UploadFile
            {
                FileName = randomFileName,
                FilePath = filePath,
                ContentType = file.ContentType,
                Format = file.ContentType switch
                {
                    "image/jpeg" => FormatEnum.Jpeg,
                    "image/png" => FormatEnum.Png,
                    "image/gif" => FormatEnum.Gif,
                    "application/pdf" => FormatEnum.Pdf,
                    _ => FormatEnum.Unknown
                },
                Size = file.Length,
            });

            Console.WriteLine("📂 SQLite DB file path: " + _context.Database.GetDbConnection().DataSource);

            await _context.SaveChangesAsync();

            return Ok(new { message = "File uploaded successfully.", filePath = filePath });
        }
    }
}
