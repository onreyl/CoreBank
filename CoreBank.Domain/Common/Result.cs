namespace Corebank.Domain.Common;

public class Result
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public string Error { get; }

    protected Result(bool isSuccess, string error)
    {
        if (isSuccess && !string.IsNullOrEmpty(error))
            throw new ArgumentException("Success result cannot have error");

        if (!isSuccess && string.IsNullOrEmpty(error))
            throw new ArgumentException("Failure result must have error");

        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result Success() => new Result(true, string.Empty);
    public static Result Failure(string error) => new Result(false, error);
}

public class Result<T> : Result
{
    public T Value { get; }

    protected Result(bool isSuccess, string error, T value)
        : base(isSuccess, error)
    {
        Value = value;
    }

    public static Result<T> Success(T value) => new Result<T>(true, string.Empty, value);
    public static new Result<T> Failure(string error) => new Result<T>(false, error, default!);
}