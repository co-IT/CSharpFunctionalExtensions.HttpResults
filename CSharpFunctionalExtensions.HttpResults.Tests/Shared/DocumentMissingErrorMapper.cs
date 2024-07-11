using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CSharpFunctionalExtensions.HttpResults.Tests.Shared;

public class DocumentMissingErrorMapper : IResultErrorMapper<DocumentMissingError, NotFound<string>>
{
    public Func<DocumentMissingError, NotFound<string>> Map => error => TypedResults.NotFound(error.DocumentId);
}