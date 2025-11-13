namespace Infrastructure.Files;

internal sealed class FileOptions
{
    public required string ConnectionString { get; init; }
    public required string DefaultContainerName { get; init; }
}