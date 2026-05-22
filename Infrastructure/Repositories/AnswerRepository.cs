using Domain.Answers;
using StackExchange.Redis;

namespace Infrastructure.Repositories;

internal sealed class AnswerRepository(IConnectionMultiplexer redis) : IAnswerRepository
{
    public async Task SubmitAnswersAsync(string date, string category, string word, CancellationToken cancellationToken = default)
    {
        IDatabase db = redis.GetDatabase();
        string key = $"answers:{date}:{category}";
        await db.SortedSetIncrementAsync(key, word, 1);
        await db.KeyExpireAsync(key, TimeSpan.FromDays(2));
    }

    public async Task<Dictionary<string, List<(string Word, int Count)>>> GetTopAnswersAsync(string date, int topN = 3, CancellationToken cancellationToken = default)
    {
        IDatabase db = redis.GetDatabase();
        IServer server = redis.GetServer(redis.GetEndPoints()[0]);

        var result = new Dictionary<string, List<(string Word, int Count)>>();

        var keys = server.Keys(pattern: $"answers:{date}:*");

        foreach (var key in keys)
        {
            string category = key.ToString().Replace($"answers:{date}:", "");
            var entries = await db.SortedSetRangeByRankWithScoresAsync(key, 0, topN - 1, Order.Descending);
            result[category] = entries.Select(e => (e.Element.ToString(), (int)e.Score)).ToList();
        }

        return result;
    }
}