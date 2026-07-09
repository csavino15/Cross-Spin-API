using Api.Abstractions;
using Application.Answers.GetTop;
using Domain.Abstractions;
using MediatR;

namespace Api.Endpoints.V1.Answers;

internal sealed class GetTopAnswersEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("answers/{date}", async (
            string date,
            ISender sender,
            CancellationToken cancellationToken = default,
            int top = 3) =>
        {
            Result<TopAnswersDTO> result = await sender.Send(
                new GetTopAnswersQuery(date, top),
                cancellationToken);

            if (result.IsFailure)
                return Results.NotFound(result.Error);

            return Results.Ok(result.Value);
        })
            .WithTags("Answers")
            .MapToApiVersion(ApiVersions.V1);
    }
}