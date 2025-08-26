using Microsoft.EntityFrameworkCore;
using StarterKit.Data;
using StarterKit.Models;
using static StarterKit.Enums.Enum;

namespace StarterKit.Services;

public interface IUploadService
{
    Task<string> UploadFile(IFormFile file, string extension);
}

public class UploadService(IServiceProvider services) : IUploadService
{
    private readonly IServiceProvider _services = services;

    public async Task<string> UploadFile(IFormFile file, string extension)
    {
        using var scope = _services.CreateScope();
        AppDbContext _context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

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

        return filePath;
    }
}