using Api.Abstractions;
using Application.Levels.ByDate;
using Application.Levels.Today;
using Domain.Abstractions;
using MediatR;

namespace Api.Endpoints.V1.Levels.ByDate;

internal sealed class GetLevelByDateEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("levels/{date}", async (
            DateOnly date,
            ISender sender,
            CancellationToken cancellationToken = default) =>
        {
            Result<LevelDTO> result = await sender.Send(
                new GetLevelByDateQuery(date), cancellationToken);

            if (result.IsFailure)
                return Results.NotFound(result.Error);

            return Results.Ok(result.Value);
        })
            .WithTags("Levels")
            .MapToApiVersion(ApiVersions.V1);
    }
}