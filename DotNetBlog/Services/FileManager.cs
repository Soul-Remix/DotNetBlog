using DotNetBlog.Interfaces;

namespace DotNetBlog.Services;

public class FileManager : IFileManager
{
    private readonly string _imagePath;

    public FileManager(IConfiguration config)
    {
        _imagePath = config["Path:Images"];
    }

    public FileStream ImageStream(string image)
    {
        return new FileStream(Path.Combine(_imagePath, image), FileMode.Open, FileAccess.Read);
    }

    public async Task<string> SaveImage(IFormFile image)
    {
        var savePath = Path.Combine(_imagePath);
        if (!Directory.Exists(savePath))
        {
            Directory.CreateDirectory(savePath);
        }

        var mime = image.FileName.Substring(image.FileName.LastIndexOf(".", StringComparison.Ordinal));
        var fileName = $"img_{DateTime.Now:s}{mime}";

        using (var fileStream = new FileStream(Path.Combine(savePath, fileName), FileMode.Create))
        {
            await image.CopyToAsync(fileStream);
        }

        return fileName;
    }
}