using Application.Abstractions.Messaging;

namespace Application.Answers.GetTop;

public record GetTopAnswersQuery(string Date, int Top = 3) : IQuery<TopAnswersDTO>;