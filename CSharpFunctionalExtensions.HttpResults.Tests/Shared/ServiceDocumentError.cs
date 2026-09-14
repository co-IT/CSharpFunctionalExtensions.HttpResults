namespace CSharpFunctionalExtensions.HttpResults.Tests.Shared;

/// <summary>Custom error used by the DI mapper fixtures.</summary>
public sealed class ServiceDocumentError
{
  public required string DocumentId { get; init; }
}
