using CSharpFunctionalExtensions.HttpResults;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace CSharpFunctionalExtensions.HttpResults.IntegrationTests.Features.Documents;

/// <summary>Service mapper resolved from request services; builds its ProblemDetails by hand.</summary>
public class DocumentNotFoundErrorMapper(DocumentationLinkProvider linkProvider)
  : IServiceResultErrorMapper<DocumentNotFoundError, ProblemHttpResult>
{
  public ProblemHttpResult Map(DocumentNotFoundError error)
  {
    var problemDetails = new ProblemDetails
    {
      Status = StatusCodes.Status404NotFound,
      Title = "Document not found",
      Type = linkProvider.For("document-not-found"),
      Detail = $"Document {error.DocumentId} could not be found.",
    };

    return TypedResults.Problem(problemDetails);
  }
}
