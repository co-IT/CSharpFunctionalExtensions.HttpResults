using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace CSharpFunctionalExtensions.HttpResults.Tests.Shared;

/// <summary>DI mapper returning ProblemHttpResult - usable as failure arm in every built-in union.</summary>
public sealed class ServiceDocumentErrorMapper(ServiceMapperDependency dependency)
  : IServiceResultErrorMapper<ServiceDocumentError, ProblemHttpResult>
{
  public ProblemHttpResult Map(ServiceDocumentError error)
  {
    var problemDetails = new ProblemDetails
    {
      Status = StatusCodes.Status404NotFound,
      Title = "Document not found",
      Detail = dependency.Prefix + error.DocumentId,
    };

    return TypedResults.Problem(problemDetails);
  }
}
