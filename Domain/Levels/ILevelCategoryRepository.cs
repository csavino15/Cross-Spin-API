namespace Domain.Levels;

public interface ILevelCategoryRepository
{
    Task<IReadOnlyCollection<LevelCategory>> GetValuesForCategory(IEnumerable<string> categories, CancellationToken cancellationToken = default);
}