namespace Domain.Answers;

public interface IAnswerRepository
{
    Task SubmitAnswersAsync(string date, string category, string word, CancellationToken cancellationToken = default);
    Task TrackCategoryAsync(string date, string category, CancellationToken cancellationToken = default);
    Task<Dictionary<string, List<(string Word, int Count)>>> GetTopAnswersAsync(string date, int topN = 3, CancellationToken cancellationToken = default);
}