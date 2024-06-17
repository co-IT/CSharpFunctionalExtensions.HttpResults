namespace CSharpFunctionalExtensions.HttpResults;

/// <summary>
/// Interface for mapping IResultError to IResult
/// </summary>
public interface IResultErrorMapper<TResultError, THttpResult> where TResultError : IResultError where THttpResult : Microsoft.AspNetCore.Http.IResult
{
    /// <summary>
    /// Map IResultError to IResult
    /// </summary>
    public Func<TResultError, THttpResult> Map { get; }
}