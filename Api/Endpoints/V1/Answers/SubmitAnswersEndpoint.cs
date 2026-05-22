using Api.Abstractions;
using Application.Answers.Submit;
using Domain.Abstractions;
using MediatR;

namespace Api.Endpoints.V1.Answers;

internal sealed class SubmitAnswersEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("answers", async (
            SubmitAnswersRequest request,
            ISender sender,
            CancellationToken cancellationToken = default) =>
        {
            Result result = await sender.Send(
                new SubmitAnswersCommand(request.Date, request.CategoryAnswers),
                cancellationToken);

            if (result.IsFailure)
                return Results.BadRequest(result.Error);

            return Results.Ok();
        })
            .WithTags("Answers")
            .MapToApiVersion(ApiVersions.V1);
    }
}

public record SubmitAnswersRequest(
    string Date,
    Dictionary<string, string> CategoryAnswers);