namespace CSharpFunctionalExtensions.HttpResults;

/// <summary>
///   Interface for mapping custom error to <see cref="Microsoft.AspNetCore.Http.IResult" />
/// </summary>
public interface IResultErrorMapper<in TError, out THttpResult>
  where THttpResult : Microsoft.AspNetCore.Http.IResult
{
  /// <summary>
  ///   Map custom error to <see cref="Microsoft.AspNetCore.Http.IResult" />
  /// </summary>
  public THttpResult Map(TError error);
}
