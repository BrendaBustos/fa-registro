namespace GestorMensajesInstitucionales.Application.Interfaces;

public interface IFileStorage
{
    Task<string> SaveAsync(string fileName, Stream content, string subfolder);
    Task DeleteAsync(string path);
    Task<IReadOnlyList<string>> ExtractCompressedAsync(string sourcePath, string destinationFolder);
}
