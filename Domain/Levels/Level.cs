namespace Domain.Levels;
public class Level
{
    public DateTime Date { get; set; }
    public IEnumerable<string> Categories { get; set; } = [];
    public IEnumerable<Word> Words { get; set; } = [];
}
