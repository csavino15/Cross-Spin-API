using Application.Abstractions.Messaging;
using Application.Levels.Today;

namespace Application.Levels.ByDate;

public record GetLevelByDateQuery(DateOnly Date) : IQuery<LevelDTO>;