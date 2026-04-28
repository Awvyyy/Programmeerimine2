namespace KooliProjekt.WinFormsApp.Api;

public class Result
{
    public bool IsSuccess { get; set; }
    public string? Error { get; set; }
    public static Result Ok() => new() { IsSuccess = true };
    public static Result Fail(string error) => new() { IsSuccess = false, Error = error };
}

public class Result<T> : Result
{
    public T? Value { get; set; }
    public static Result<T> Ok(T value) => new() { IsSuccess = true, Value = value };
    public new static Result<T> Fail(string error) => new() { IsSuccess = false, Error = error };
}
