using Domain.Abstractions;

namespace Domain.Answers;

public static class AnswerErrors
{
    public static readonly Error NotFound = new(
        "Answers.NotFound",
        "No answers found for the specified date.");
}