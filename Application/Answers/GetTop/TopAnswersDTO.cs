namespace Application.Answers.GetTop;

public class TopAnswersDTO
{
    public string Date { get; set; } = string.Empty;
    public Dictionary<string, List<TopWordDTO>> Categories { get; set; } = new();
}

public class TopWordDTO
{
    public string Word { get; set; } = string.Empty;
    public int Count { get; set; }
}