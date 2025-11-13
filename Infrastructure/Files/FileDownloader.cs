using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Infrastructure.Files;
internal sealed class FileDownloader : IFileDownloader
{
    private readonly BlobContainerClient _blobContainerClient;

    public FileDownloader(IOptions<FileOptions> options)
    {
        ArgumentNullException.ThrowIfNull(options.Value);
        BlobServiceClient fileStorageClient = new BlobServiceClient(options.Value.ConnectionString);
        _blobContainerClient = fileStorageClient.GetBlobContainerClient(options.Value.DefaultContainerName);
    }

    private async Task<BinaryData?> GetFileContents(string fileName, CancellationToken cancellationToken = default)
    {
        BlobClient client = _blobContainerClient.GetBlobClient(fileName);

        try
        {
            Response<BlobDownloadResult> resp = await client.DownloadContentAsync(cancellationToken);
            return resp.Value.Content;
        }
        catch (RequestFailedException ex) when (ex.Status == 404)
        {
            // Blob not found — return default(T) or throw, your call
            return default;
        }
    }
    public async Task<T?> GetFileContents<T>(string fileName, CancellationToken cancellationToken = default)
    {
        BinaryData? data = await GetFileContents(fileName, cancellationToken);
        if (data == null)
            return default;

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        return data.ToObjectFromJson<T>(options);
    }
    public async Task<string> GetFileContentsAsString(string fileName, CancellationToken cancellationToken = default)
    {
        BinaryData? data = await GetFileContents(fileName, cancellationToken);
        if (data == null)
            return string.Empty;
        return data.ToString();
    }
}
