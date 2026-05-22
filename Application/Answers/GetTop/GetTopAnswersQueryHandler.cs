using Application.Abstractions.Messaging;
using Domain.Abstractions;
using Domain.Answers;

namespace Application.Answers.GetTop;

internal sealed class GetTopAnswersQueryHandler(
    IAnswerRepository answerRepository) : IQueryHandler<GetTopAnswersQuery, TopAnswersDTO>
{
    public async Task<Result<TopAnswersDTO>> Handle(GetTopAnswersQuery request, CancellationToken cancellationToken)
    {
        var topAnswers = await answerRepository.GetTopAnswersAsync(request.Date, 3, cancellationToken);

        if (topAnswers == null || topAnswers.Count == 0)
            return Result.Failure<TopAnswersDTO>(AnswerErrors.NotFound);

        TopAnswersDTO dto = new()
        {
            Date = request.Date,
            Categories = topAnswers.ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value.Select(w => new TopWordDTO
                {
                    Word = w.Word,
                    Count = w.Count
                }).ToList()
            )
        };

        return Result.Success(dto);
    }
}