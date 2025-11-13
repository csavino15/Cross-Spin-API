using Domain.Levels;
using Infrastructure.Files;
using Infrastructure.Levels;
using Microsoft.Extensions.Caching.Hybrid;

namespace Infrastructure.Repositories;

internal sealed class LevelCategoryRepository(HybridCache cache, IFileDownloader fileDownloader) : ILevelCategoryRepository
{
    private static readonly string[] FILES = [
        "BRE1-Categories.csv",
        "BRE1-Contains.csv",
        "BRE1-StartsWith.csv",
    ];

    public async Task<IReadOnlyCollection<LevelCategory>> GetValuesForCategory(IEnumerable<string> categories, CancellationToken cancellationToken = default)
    {
        HashSet<LevelCategory> found = new();
        foreach (string file in FILES)
        {
            string content = await cache.GetOrCreateAsync(file, async (ct) => 
            { 
                return await fileDownloader.GetFileContentsAsString(file, cancellationToken); 
            }, cancellationToken: cancellationToken);
            var parsed = LevelCategoryParser.ParseColumns(content);
            foreach (KeyValuePair<string, List<string>> kvp in parsed)
            {
                found.Add(new LevelCategory
                {
                    Key = kvp.Key,
                    Values = kvp.Value
                });
            }
        }
        return [.. found.Where(f => categories.Contains(f.Key))];
    }
}
