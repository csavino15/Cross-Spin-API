using Application.Abstractions.Messaging;

namespace Application.Answers.Submit;

public record SubmitAnswersCommand(
    string Date,
    Dictionary<string, string> CategoryAnswers) : ICommand;