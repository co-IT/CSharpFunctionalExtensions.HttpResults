using CSharpFunctionalExtensions.MinimalApi;

namespace WebApi.CustomResultErrors.DocumentMissingResult;

public class DocumentMissingResultError : IResultError
{
    public required Guid DocumentId { get; init; }
    public required string Details { get; init; }
}