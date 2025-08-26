using Microsoft.AspNetCore.Mvc;
using StarterKit.Services;

namespace StarterKit.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UploadController(IUploadService uploadService) : Controller
{
    private readonly IUploadService _uploadService = uploadService;

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

        string? filePath = await _uploadService.UploadFile(file, extension);
        if (filePath == null)
        {
            return StatusCode(500, "An error occurred while uploading the file.");
        }

        return Ok(new { message = "File uploaded successfully.", filePath });
    }
}
