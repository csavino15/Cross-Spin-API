namespace Domain.Abstractions;
public record GenericError(string Code, string Name)
{
    public static readonly GenericError None = new(string.Empty, string.Empty);
    public static readonly GenericError IdMismatch = new("General.IdMismatch", "Input and logged in user's Id did not match");
    public static readonly GenericError NullValue = new("Error.NullValue", "Null value was provided");
    public static readonly GenericError TransactionError = new("Error.TransactionError", "Something went wrong updating the records");
}
