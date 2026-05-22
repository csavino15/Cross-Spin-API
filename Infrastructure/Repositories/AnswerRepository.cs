using Domain.Answers;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Infrastructure.Repositories;

internal sealed class AnswerRepository(IDistributedCache cache) : IAnswerRepository
{
    public async Task SubmitAnswersAsync(string date, string category, string word, CancellationToken cancellationToken = default)
    {
        string key = $"answers:{date}:{category}";
        var existing = await cache.GetStringAsync(key, cancellationToken);
        Dictionary<string, int> counts = existing != null
            ? JsonSerializer.Deserialize<Dictionary<string, int>>(existing) ?? new()
            : new();

        counts[word] = counts.TryGetValue(word, out int count) ? count + 1 : 1;

        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(2)
        };
        await cache.SetStringAsync(key, JsonSerializer.Serialize(counts), options, cancellationToken);
    }

    public async Task<Dictionary<string, List<(string Word, int Count)>>> GetTopAnswersAsync(string date, int topN = 3, CancellationToken cancellationToken = default)
    {
        var result = new Dictionary<string, List<(string Word, int Count)>>();
        // We need to store category keys separately so we can look them up
        string indexKey = $"answers:{date}:_index";
        var indexData = await cache.GetStringAsync(indexKey, cancellationToken);
        List<string> categories = indexData != null
            ? JsonSerializer.Deserialize<List<string>>(indexData) ?? new()
            : new();

        foreach (var category in categories)
        {
            string key = $"answers:{date}:{category}";
            var data = await cache.GetStringAsync(key, cancellationToken);
            if (data == null) continue;

            var counts = JsonSerializer.Deserialize<Dictionary<string, int>>(data) ?? new();
            result[category] = counts
                .OrderByDescending(kvp => kvp.Value)
                .Take(topN)
                .Select(kvp => (kvp.Key, kvp.Value))
                .ToList();
        }

        return result;
    }

    public async Task TrackCategoryAsync(string date, string category, CancellationToken cancellationToken = default)
    {
        string indexKey = $"answers:{date}:_index";
        var indexData = await cache.GetStringAsync(indexKey, cancellationToken);
        List<string> categories = indexData != null
            ? JsonSerializer.Deserialize<List<string>>(indexData) ?? new()
            : new();

        if (!categories.Contains(category))
        {
            categories.Add(category);
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(2)
            };
            await cache.SetStringAsync(indexKey, JsonSerializer.Serialize(categories), options, cancellationToken);
        }
    }
}