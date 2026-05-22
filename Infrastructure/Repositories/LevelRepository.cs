using Application.Abstractions.Clock;
using Domain.Levels;
using Infrastructure.Files;
using Infrastructure.Levels;
using Microsoft.Extensions.Caching.Hybrid;

namespace Infrastructure.Repositories;
internal sealed class LevelRepository(HybridCache cache, IFileDownloader fileDownloader, IDateTimeProvider dateTimeProvider) : ILevelRepository
{
    private const string DAILY_LEVELS_FILENAME = "BRE1-Levels-Daily.csv";

    public async Task<IEnumerable<Level>> GetAllLevels(CancellationToken cancellationToken = default)
    {
        string content = await cache.GetOrCreateAsync(DAILY_LEVELS_FILENAME, async (ct) =>
        {
            return await fileDownloader.GetFileContentsAsString(DAILY_LEVELS_FILENAME, cancellationToken);
        }, cancellationToken: cancellationToken);
        return LevelParser.Parse(content);
    }

    public async Task<Level?> GetLevelForDate(DateOnly dateForLevel, CancellationToken cancellationToken = default)
    {
        IEnumerable<Level> levels = await GetAllLevels(cancellationToken);
        return levels.FirstOrDefault(level => level.Date == dateForLevel.ToDateTime(TimeOnly.MinValue).Date);
    }
}
