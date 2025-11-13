
namespace Infrastructure.Files;

internal interface IFileDownloader
{
    Task<T?> GetFileContents<T>(string fileName, CancellationToken cancellationToken = default);
    Task<string> GetFileContentsAsString(string fileName, CancellationToken cancellationToken = default);
}