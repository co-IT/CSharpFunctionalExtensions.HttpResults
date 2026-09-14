namespace CSharpFunctionalExtensions.HttpResults;

/// <summary>
///   Marks an error mapper as a request-service-resolved mapper. Implementations may use constructor injection.
///   The generated extension methods require an <see cref="Microsoft.AspNetCore.Http.HttpContext" /> argument to
///   resolve the mapper from <see cref="Microsoft.AspNetCore.Http.HttpContext.RequestServices" /> on failure.
/// </summary>
public interface IServiceResultErrorMapper<in TError, out THttpResult> : IResultErrorMapper<TError, THttpResult>
  where THttpResult : Microsoft.AspNetCore.Http.IResult;
