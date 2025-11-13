using Microsoft.Extensions.Logging;
using Quartz;

namespace Infrastructure.Jobs.ResetLeaderboard;

internal sealed class ResetLeaderboardJob(HttpClient httpClient, ILogger<ResetLeaderboardJob> logger) : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        const string url = "https://crossspin-functions.azurewebsites.net/api/TriggerLeaderboardUpdate";

        try
        {
            var response = await httpClient.GetAsync(new Uri(url), context.CancellationToken);
            response.EnsureSuccessStatusCode();
            logger.LogInformation("[{Timestamp}] ✅ Successfully triggered leaderboard update.", DateTime.Now);
        }
        catch (Exception ex)
        {
            logger.LogCritical(new EventId(), ex, "[{Timestamp}] ❌ Failed to trigger leaderboard update: {Message}", DateTime.Now, ex.Message);
        }
    }
}
