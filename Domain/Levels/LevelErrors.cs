using Domain.Abstractions;

namespace Domain.Levels;

public static class LevelErrors
{
    public static readonly GenericError LevelNotFound = new GenericError("Level.NotFound", "Level was not found.");
}