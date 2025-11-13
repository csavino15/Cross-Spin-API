namespace Domain.Levels;
public interface ILevelRepository
{
    Task<Level?> GetLevelForDate(DateOnly dateForLevel, CancellationToken cancellationToken = default);
    Task<IEnumerable<Level>> GetAllLevels(CancellationToken cancellationToken = default);
}
