using Application.Abstractions.Clock;
using Domain.Levels;
using Infrastructure.Clock;
using Infrastructure.Files;
using Infrastructure.Jobs.ResetLeaderboard;
using Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using Polly.Retry;
using Quartz;

namespace Infrastructure;
public static class InfrastructureDependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);
        AddLogging(services);
        AddServices(services);
        AddRepositories(services);
        Configure(services, configuration);
        AddBackgroundJobs(services);
        AddPolly(services);
    }

    private static void AddLogging(IServiceCollection services)
    {
        services.AddLogging();
    }

    private static void AddBackgroundJobs(IServiceCollection services) 
    {
        services.AddQuartz();
        services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);

        services.ConfigureOptions<ResetLeaderboardJobSetup>();
    }

    private static void AddServices(IServiceCollection services)
    {
        services.AddScoped<IFileDownloader, FileDownloader>();
        services.AddScoped<IDateTimeProvider, DateTimeProvider>();
    }

    private static void AddRepositories(IServiceCollection services)
    {
        services.AddScoped<ILevelCategoryRepository, LevelCategoryRepository>();
        services.AddScoped<ILevelRepository, LevelRepository>();
    }

    private static void Configure(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<Files.FileOptions>(configuration.GetSection("Files"));
        services.Configure<TimeZoneOptions>(configuration.GetSection("TimeZone"));
    }

    private static void AddPolly(IServiceCollection services)
    {
        services.AddHttpClient<ResetLeaderboardJob>();
    }

    private static AsyncRetryPolicy<HttpResponseMessage> GetRetryPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError() // Handles 5xx, 408 and HttpRequestException
            .WaitAndRetryAsync(
                retryCount: 3,
                sleepDurationProvider: attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)), // 2s, 4s, 8s
                onRetry: (outcome, timespan, attempt, context) =>
                {
                    Console.WriteLine($"Retry {attempt} after {timespan.TotalSeconds}s due to: " +
                                      $"{outcome.Exception?.Message ?? outcome.Result.StatusCode.ToString()}");
                });
    }
}
