namespace CSharpFunctionalExtensions.HttpResults.Tests.Shared;

public class DocumentMissingError : IResultError
{
    public required string DocumentId { get; init; }
}