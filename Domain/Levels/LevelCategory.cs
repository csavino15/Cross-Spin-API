namespace Domain.Levels;

public class LevelCategory
{
    public string? Key { get; set; }
    public IEnumerable<string> Values { get; set; } = [];
}