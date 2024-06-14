namespace CSharpFunctionalExtensions.HttpResults;

public interface IResultErrorMapper<TResultError, THttpResult> where TResultError : IResultError where THttpResult : Microsoft.AspNetCore.Http.IResult
{
    public Func<TResultError, THttpResult> Map { get; }
}