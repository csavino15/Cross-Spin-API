namespace Domain.Levels;

public class Word
{
    public Word()
    {
        
    }
    public Word(IEnumerable<string> coordinates, int length)
    {
        Coordinates = coordinates;
        Length = length;
    }

    public int Length { get; set; }
    public IEnumerable<string> Coordinates { get; set; } = [];
}