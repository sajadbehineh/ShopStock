using SkiaSharp;
using ShopStock.Application.Contracts;

namespace ShopStock.Application.Services;

public class ImageService : IImageService
{
    private readonly string _baseStoragePath;

    public ImageService()
    {
        // مسیر ذخیره‌سازی در پوشه ای خارج از پروژه وب
        _baseStoragePath = Path.Combine(Directory.GetCurrentDirectory(), "..", "Uploads_Storage");
    }

    public async Task<string> SaveImageAsync(Stream fileStream, string folderName, int width = 512, int height = 512)
    {
        var targetPath = Path.Combine(_baseStoragePath, folderName);
        if (!Directory.Exists(targetPath)) Directory.CreateDirectory(targetPath);

        // Decoding & Security Check
        using var managedStream = new SKManagedStream(fileStream);
        using var bitmap = SKBitmap.Decode(managedStream);

        if (bitmap == null) throw new InvalidDataException("فایل ارسالی یک تصویر معتبر نیست.");
        if (bitmap.Width > 5000 || bitmap.Height > 5000) throw new Exception("ابعاد تصویر غیرمجاز است.");

        // Resizing with NEW API (No Warnings)
        var info = new SKImageInfo(width, height);
        using var resizedBitmap = bitmap.Resize(info, new SKSamplingOptions(SKFilterMode.Linear, SKMipmapMode.None));

        var fileName = $"{Guid.NewGuid():N}.jpg";
        var fullPath = Path.Combine(targetPath, fileName);

        // Encoding & Metadata Removal
        using var image = SKImage.FromBitmap(resizedBitmap);
        using var data = image.Encode(SKEncodedImageFormat.Jpeg, 80);

        await using var stream = File.OpenWrite(fullPath);
        data.SaveTo(stream);

        return fileName;
    }

    public void DeleteImage(string fileName, string folderName)
    {
        var fullPath = Path.Combine(_baseStoragePath, folderName, fileName);
        if (File.Exists(fullPath)) File.Delete(fullPath);
    }
}