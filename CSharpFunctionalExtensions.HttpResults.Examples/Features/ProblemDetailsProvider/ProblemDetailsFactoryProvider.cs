using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace CSharpFunctionalExtensions.HttpResults.Examples.Features.ProblemDetailsProvider;

/// <summary>
///   Example integration with ASP.NET Core's <see cref="ProblemDetailsFactory" />. The generator discovers
///   this provider and registers it as a scoped <see cref="IResultProblemDetailsProvider" /> automatically.
/// </summary>
public sealed class ProblemDetailsFactoryProvider(ProblemDetailsFactory problemDetailsFactory)
  : IResultProblemDetailsProvider
{
  public ProblemDetails CreateProblemDetails(HttpContext httpContext, string error, int statusCode)
  {
    var problemDetails = problemDetailsFactory.CreateProblemDetails(httpContext, statusCode, detail: error);
    problemDetails.Extensions["source"] = "custom-provider";
    return problemDetails;
  }
}
