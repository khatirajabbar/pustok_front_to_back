using pustok_front_to_back.Services.Interfaces;

namespace pustok_front_to_back.Services;

public class FileUploadService : IFileUploadService
{
    private readonly IWebHostEnvironment _hostEnvironment;

    public FileUploadService(IWebHostEnvironment hostEnvironment)
    {
        _hostEnvironment = hostEnvironment;
    }

    public async Task<string> UploadImageAsync(IFormFile file, string folderName)
    {
        if (file == null || file.Length == 0)
            throw new ArgumentException("File is empty");

        var uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "uploads", folderName);
        
        if (!Directory.Exists(uploadsFolder))
            Directory.CreateDirectory(uploadsFolder);

        var uniqueFileName = $"{Guid.NewGuid()}_{file.FileName}";
        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

        using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(fileStream);
        }

        return $"/uploads/{folderName}/{uniqueFileName}";
    }

    public async Task DeleteImageAsync(string imagePath)
    {
        if (string.IsNullOrEmpty(imagePath))
            return;

        var filePath = Path.Combine(_hostEnvironment.WebRootPath, imagePath.TrimStart('/'));
        
        if (File.Exists(filePath))
            File.Delete(filePath);

        await Task.CompletedTask;
    }
}