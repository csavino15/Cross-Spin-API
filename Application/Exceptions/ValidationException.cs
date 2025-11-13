namespace Application.Exceptions;

public sealed class ValidationException : Exception
{
    public ValidationException(IEnumerable<ValidationError> errors)
    {
        Errors = errors;
    }

    public required IEnumerable<ValidationError> Errors { get; set; }
    public ValidationException()
    {
    }

    public ValidationException(string message) : base(message)
    {
    }

    public ValidationException(string message, Exception innerException) : base(message, innerException)
    {
    }
}