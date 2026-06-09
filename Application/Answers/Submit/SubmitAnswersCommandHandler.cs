using Application.Abstractions.Messaging;
using Domain.Abstractions;
using Domain.Answers;

namespace Application.Answers.Submit;

internal sealed class SubmitAnswersCommandHandler(
    IAnswerRepository answerRepository) : ICommandHandler<SubmitAnswersCommand>
{
    public async Task<Result> Handle(SubmitAnswersCommand request, CancellationToken cancellationToken)
    {
        foreach (var (category, word) in request.CategoryAnswers)
        {
            await answerRepository.TrackCategoryAsync(request.Date, category, cancellationToken);
            await answerRepository.SubmitAnswersAsync(request.Date, category, word.ToUpperInvariant(), cancellationToken);
        }

        return Result.Success();
    }
}