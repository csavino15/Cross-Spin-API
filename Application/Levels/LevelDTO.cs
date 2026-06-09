using Domain.Levels;
using Application.Levels.Today;

namespace Application.Levels;
public class LevelDTO
{
    public Level? Level { get; set; }
    public IReadOnlyCollection<LevelCategory> Categories { get; set; } = [];
}
