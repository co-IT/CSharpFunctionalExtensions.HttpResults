using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CSharpFunctionalExtensions.HttpResults.Tests.Shared;

/// <summary>Records every call and returns distinguishable problem details.</summary>
public sealed class RecordingResultProblemDetailsProvider(Action<ProblemDetails>? additionalConfiguration = null)
  : IResultProblemDetailsProvider
{
  private int _calls;

  internal HttpContext? ReceivedContext { get; private set; }
  internal string? ReceivedError { get; private set; }
  internal int ReceivedStatusCode { get; private set; }
  internal int Calls => _calls;

  public ProblemDetails CreateProblemDetails(HttpContext httpContext, string error, int statusCode)
  {
    _calls++;
    ReceivedContext = httpContext;
    ReceivedError = error;
    ReceivedStatusCode = statusCode;

    var problemDetails = new ProblemDetails
    {
      Status = statusCode,
      Title = TestHttpContextHelper.ProviderTitle,
      Type = TestHttpContextHelper.ProviderType,
      Detail = error,
    };
    additionalConfiguration?.Invoke(problemDetails);

    return problemDetails;
  }
}
