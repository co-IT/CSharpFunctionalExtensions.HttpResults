namespace CSharpFunctionalExtensions.HttpResults.Tests.Shared;

/// <summary>Distinct error so both DI mappers do not collide.</summary>
public sealed class ThrowingCtorError
{
  public required string DocumentId { get; init; }
}
