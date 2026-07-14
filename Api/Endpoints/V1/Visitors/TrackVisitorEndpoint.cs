using Api.Abstractions;
using Asp.Versioning;
using Microsoft.Extensions.Caching.Distributed;
using System.Globalization;
using System.Text.Json;

namespace Api.Endpoints.V1.Visitors;

internal sealed class TrackVisitorEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        // POST: increment visitor count (deduped by visitorId)
        app.MapPost("visitors/{visitorId}", async (
            string visitorId,
            IDistributedCache cache,
            CancellationToken cancellationToken = default) =>
        {
            var cst = TimeZoneInfo.FindSystemTimeZoneById("America/Chicago");
            var today = DateOnly.FromDateTime(TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, cst))
                .ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            var key = $"visitors:{today}";

            var existing = await cache.GetStringAsync(key, cancellationToken);
            var ids = existing != null
                ? JsonSerializer.Deserialize<HashSet<string>>(existing) ?? new()
                : new HashSet<string>();

            ids.Add(visitorId);

            await cache.SetStringAsync(key, JsonSerializer.Serialize(ids),
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(2)
                }, cancellationToken);

            return Results.Ok(new { count = ids.Count });
        })
        .WithTags("Visitors")
        .MapToApiVersion(new ApiVersion(1));

        // GET: read current visitor count without incrementing
        app.MapGet("visitors", async (
            IDistributedCache cache,
            CancellationToken cancellationToken = default) =>
        {
            var cst = TimeZoneInfo.FindSystemTimeZoneById("America/Chicago");
            var today = DateOnly.FromDateTime(TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, cst))
                .ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            var key = $"visitors:{today}";

            var existing = await cache.GetStringAsync(key, cancellationToken);
            var ids = existing != null
                ? JsonSerializer.Deserialize<HashSet<string>>(existing) ?? new()
                : new HashSet<string>();

            return Results.Ok(new { count = ids.Count });
        })
        .WithTags("Visitors")
        .MapToApiVersion(new ApiVersion(1));
    }
}