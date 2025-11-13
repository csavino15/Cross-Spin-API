using Application.Abstractions.Clock;
using Application.Abstractions.Messaging;
using Domain.Abstractions;
using Domain.Levels;

namespace Application.Levels.Today;

internal sealed class GetTodaysLevelQueryHandler(ILevelRepository levelRepository, ILevelCategoryRepository levelCategoryRepository, IDateTimeProvider dateTimeProvider) : IQueryHandler<GetTodaysLevelQuery, LevelDTO>
{
    public async Task<Result<LevelDTO>> Handle(GetTodaysLevelQuery request, CancellationToken cancellationToken)
    {
        Level? level = await levelRepository.GetLevelForDate(DateOnly.FromDateTime(dateTimeProvider.UtcNow), cancellationToken);
        if (level == null)
            return Result.Failure<LevelDTO>(LevelErrors.LevelNotFound);

        LevelDTO dto = new()
        {
            Level = level,
            Categories = await levelCategoryRepository.GetValuesForCategory(level.Categories, cancellationToken)
        };
        return Result.Success(dto);
    }
}
