using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CSharpFunctionalExtensions.HttpResults.Tests.Shared;

public class DocumentMissingErrorMapper : IResultErrorMapper<DocumentMissingError, NotFound<string>>
{
  public NotFound<string> Map(DocumentMissingError error) => TypedResults.NotFound(error.DocumentId);
}
