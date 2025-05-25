namespace FluentLauncher.Core.Models;

public class Result<T>
{
    public T Value { get; }
    public string? Error { get; }
    public bool IsSuccess => Error == null;

    private Result(T value, string? error)
    {
        Value = value;
        Error = error;
    }

    // 修改: 添加对可空类型的显式支持
    public static Result<T> Success(T value) => new(value, null);
    public static Result<T?> Failure(string? error) => new(default, error);
}