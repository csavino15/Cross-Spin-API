using Api.Abstractions;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Api.Endpoints.V1.Visitors;

internal sealed class TrackVisitorEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("visitors/{visitorId}", async (
            string visitorId,
            IDistributedCache cache,
            CancellationToken cancellationToken = default) =>
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow).ToString("yyyy-MM-dd");
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
        .MapToApiVersion(1);
    }
}