using Microsoft.Extensions.Logging;
using Quartz;

namespace Infrastructure.Jobs.AwardMedals;

internal sealed class AwardMedalsJob(HttpClient httpClient, ILogger<AwardMedalsJob> logger) : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        const string url = "https://crossspin-functions.azurewebsites.net/api/AwardMedals";
        try
        {
            var response = await httpClient.GetAsync(new Uri(url), context.CancellationToken);
            response.EnsureSuccessStatusCode();
            logger.LogInformation("[{Timestamp}] ✅ Successfully triggered medal award.", DateTime.Now);
        }
        catch (Exception ex)
        {
            logger.LogCritical(new EventId(), ex, "[{Timestamp}] ❌ Failed to trigger medal award: {Message}", DateTime.Now, ex.Message);
        }
    }
}