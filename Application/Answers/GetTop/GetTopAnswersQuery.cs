using Application.Abstractions.Messaging;

namespace Application.Answers.GetTop;

public record GetTopAnswersQuery(string Date) : IQuery<TopAnswersDTO>;