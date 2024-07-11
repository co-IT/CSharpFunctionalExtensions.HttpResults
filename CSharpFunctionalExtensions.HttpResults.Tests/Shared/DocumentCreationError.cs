namespace CSharpFunctionalExtensions.HttpResults.Tests.Shared;

public class DocumentCreationError : IResultError
{
    public required string DocumentId { get; init; }
}