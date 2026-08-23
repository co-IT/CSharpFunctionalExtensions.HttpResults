using CSharpFunctionalExtensions.HttpResults;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace CSharpFunctionalExtensions.HttpResults.IntegrationTests.TestApp;

/// <summary>
///   Issue-#36 integration: creates the failure ProblemDetails synchronously via MVC's
///   <see cref="ProblemDetailsFactory" />, honoring ClientErrorMapping and custom factories.
/// </summary>
public sealed class MvcFactoryProblemDetailsProvider(ProblemDetailsFactory problemDetailsFactory)
  : IResultProblemDetailsProvider
{
  public ProblemDetails CreateProblemDetails(HttpContext httpContext, string error, int statusCode)
  {
    var problemDetails = problemDetailsFactory.CreateProblemDetails(httpContext, statusCode, detail: error);
    problemDetails.Extensions["source"] = "mvc-problemdetails-factory";
    return problemDetails;
  }
}
