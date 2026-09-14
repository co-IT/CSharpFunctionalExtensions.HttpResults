using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CSharpFunctionalExtensions.HttpResults;

/// <summary>
///   Creates the failure <see cref="ProblemDetails" /> for built-in
///   <see cref="CSharpFunctionalExtensions.Result" /> and <see cref="CSharpFunctionalExtensions.Result{T}" />
///   mappings. Implement this interface to opt in to request-service-aware mapping. When exactly one valid
///   implementation is visible, generated context overloads and scoped registration are enabled.
/// </summary>
public interface IResultProblemDetailsProvider
{
  /// <summary>
  ///   Creates the failure problem details for the given error and status code.
  /// </summary>
  /// <param name="httpContext">The current request context. Can be used to access request services or request information.</param>
  /// <param name="error">The error string of the failed <see cref="CSharpFunctionalExtensions.Result" />.</param>
  /// <param name="statusCode">The failure status code of the HTTP response.</param>
  public ProblemDetails CreateProblemDetails(HttpContext httpContext, string error, int statusCode);
}
