namespace pustok_front_to_back.Services.Interfaces;

public interface IFileUploadService
{
    Task<string> UploadImageAsync(IFormFile file, string folderName);
    Task DeleteImageAsync(string imagePath);
}