using System.IO.Compression;
using GestorMensajesInstitucionales.Application.Interfaces;

namespace GestorMensajesInstitucionales.Infrastructure.Storage;

public class FileSystemStorage : IFileStorage
{
    private readonly string _basePath;

    public FileSystemStorage(string basePath)
    {
        _basePath = basePath;
        Directory.CreateDirectory(_basePath);
    }

    public async Task<string> SaveAsync(string fileName, Stream content, string subfolder)
    {
        var folder = Path.Combine(_basePath, subfolder);
        Directory.CreateDirectory(folder);
        var fullPath = Path.Combine(folder, $"{Guid.NewGuid()}_{fileName}");
        await using var fileStream = File.Create(fullPath);
        await content.CopyToAsync(fileStream);
        return fullPath;
    }

    public Task DeleteAsync(string path)
    {
        if (File.Exists(path))
        {
            File.Delete(path);
        }
        return Task.CompletedTask;
    }

    public Task<IReadOnlyList<string>> ExtractCompressedAsync(string sourcePath, string destinationFolder)
    {
        Directory.CreateDirectory(destinationFolder);
        var extension = Path.GetExtension(sourcePath).ToLowerInvariant();
        if (extension == ".zip")
        {
            ZipFile.ExtractToDirectory(sourcePath, destinationFolder, overwriteFiles: true);
            return Task.FromResult((IReadOnlyList<string>)Directory.GetFiles(destinationFolder, "*", SearchOption.AllDirectories));
        }

        if (extension == ".rar")
        {
            throw new NotSupportedException("La extracción de RAR requiere un descompresor externo (ej. UnRAR). Configure la ruta antes de usar.");
        }

        throw new InvalidOperationException("Tipo de archivo comprimido no soportado");
    }
}
