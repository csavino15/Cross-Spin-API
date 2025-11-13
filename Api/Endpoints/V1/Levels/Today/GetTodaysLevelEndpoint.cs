using Api.Abstractions;
using Application.Abstractions.Clock;
using Application.Levels;
using Application.Levels.Today;
using Domain.Abstractions;
using MediatR;

namespace Api.Endpoints.V1.Levels.Today;

internal sealed class GetTodaysLevelEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("levels/today", async (
            IDateTimeProvider dateTimeProvider,
            ISender sender,
            CancellationToken cancellationToken = default) =>
        {

            TimeSpan expiration = dateTimeProvider.UtcNow.AddDays(1).Date.AddMinutes(-5) - dateTimeProvider.UtcNow;
            Result<LevelDTO> result = await sender.Send(new GetTodaysLevelQuery($"Level-{dateTimeProvider.UtcNow:ddMMyyyy}", expiration), cancellationToken);
            if (result.IsFailure)
                return Results.BadRequest(result.Error);
            return Results.Ok(result.Value);
        })
            .WithTags("Levels")
            .MapToApiVersion(ApiVersions.V1);
    }
}
