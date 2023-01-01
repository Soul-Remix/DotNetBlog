using DotNetBlog.Interfaces;
using PhotoSauce.MagicScaler;

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
            MagicImageProcessor.ProcessImage(image.OpenReadStream(), fileStream, _imageSettings);
        }

        return fileName;
    }

    public bool RemoveImage(string image)
    {
        var file = Path.Combine(_imagePath, image);
        if (!File.Exists(file))
        {
            return false;
        }

        File.Delete(file);
        return true;
    }

    private ProcessImageSettings _imageSettings = new ProcessImageSettings()
    {
        Width = 1024,
        Height = 1024,
        ResizeMode = CropScaleMode.Crop,
        EncoderOptions = new JpegEncoderOptions()
        {
            Quality = 100,
            Subsample = ChromaSubsampleMode.Subsample420,
        },
    };
}