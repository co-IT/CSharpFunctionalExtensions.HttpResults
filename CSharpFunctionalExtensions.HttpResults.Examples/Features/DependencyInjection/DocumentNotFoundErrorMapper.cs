using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace CSharpFunctionalExtensions.HttpResults.Examples.Features.DependencyInjection;

/// <summary>
///   Service variant of an error mapper: resolved from request services, so constructor injection works.
/// </summary>
public sealed class DocumentNotFoundErrorMapper(DocumentationLinkProvider linkProvider)
  : IServiceResultErrorMapper<DocumentNotFoundError, ProblemHttpResult>
{
  public ProblemHttpResult Map(DocumentNotFoundError error)
  {
    var problemDetails = new ProblemDetails
    {
      Status = StatusCodes.Status404NotFound,
      Title = "Document not found",
      Type = linkProvider.For("document-not-found"),
      Detail = $"Document with Id {error.DocumentId} could not be found.",
    };

    return TypedResults.Problem(problemDetails);
  }
}
