using System.Diagnostics.CodeAnalysis;

namespace Domain.Abstractions;

public class Result
{
    protected internal Result(bool isSuccess, GenericError error)
    {
        if (isSuccess && error != GenericError.None)
        {
            throw new InvalidOperationException();
        }

        if (!isSuccess && error == GenericError.None)
        {
            throw new InvalidOperationException();
        }

        IsSuccess = isSuccess;
        Error = error;
    }

    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public GenericError Error { get; }
    public static Result Success() => new Result(true, GenericError.None);
    public static Result Failure(GenericError error) => new Result(false, error);
    public static Result<TValue> Success<TValue>(TValue value) => new(value, true, GenericError.None);
    public static Result<TValue> Failure<TValue>(GenericError error) => new(default, false, error);
    public static Result<TValue> Create<TValue>(TValue value) => value is not null ? Success(value) : Failure<TValue>(GenericError.NullValue);

    public static Result<T> Failure<T>(object userErrors)
    {
        throw new NotImplementedException();
    }
}

public class Result<TValue> : Result
{
    private readonly TValue? _value;

    protected internal Result(TValue? value, bool isSuccess, GenericError error)
        : base(isSuccess, error)
    {
        _value = value;
    }

    [NotNull]
    public TValue Value => IsSuccess
        ? _value!
        : throw new InvalidOperationException("The value of a failure result can not be accessed");

    public static implicit operator Result<TValue>(TValue? value)
    {
        ArgumentNullException.ThrowIfNull(value, nameof(value));
        return Create(value);
    }
}