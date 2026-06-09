using Application.Abstractions.Messaging;
using Application.Levels;
using Domain.Abstractions;
using Domain.Levels;

namespace Application.Levels.ByDate;

internal sealed class GetLevelByDateQueryHandler(
    ILevelRepository levelRepository,
    ILevelCategoryRepository levelCategoryRepository) : IQueryHandler<GetLevelByDateQuery, LevelDTO>
{
    public async Task<Result<LevelDTO>> Handle(GetLevelByDateQuery request, CancellationToken cancellationToken)
    {
        Level? level = await levelRepository.GetLevelForDate(request.Date, cancellationToken);
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