using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CSharpFunctionalExtensions.HttpResults.Tests.Shared;

public class DocumentCreationErrorMapper : IResultErrorMapper<DocumentCreationError, Conflict<string>>
{
    public Func<DocumentCreationError, Conflict<string>> Map => error => TypedResults.Conflict(error.DocumentId);
}