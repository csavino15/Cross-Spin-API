namespace Domain.Answers;

public interface IAnswerRepository
{
    Task SubmitAnswersAsync(string puzzleDate, string category, string word, CancellationToken cancellationToken = default);
    Task TrackCategoryAsync(string puzzleDate, string category, CancellationToken cancellationToken = default);
    Task<Dictionary<string, List<(string Word, int Count)>>> GetTopAnswersAsync(string puzzleDate, int topN = 3, CancellationToken cancellationToken = default);
}