using Application.Abstractions.Messaging;

namespace Application.Levels.Today;
public record GetTodaysLevelQuery(string CacheKey, TimeSpan? Expiration) : IQuery<LevelDTO>;
