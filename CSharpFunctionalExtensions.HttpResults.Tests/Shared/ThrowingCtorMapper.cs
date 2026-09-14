using Microsoft.AspNetCore.Http.HttpResults;

namespace CSharpFunctionalExtensions.HttpResults.Tests.Shared;

/// <summary>Throws on construction - proves that success paths never resolve mappers.</summary>
public sealed class ThrowingCtorMapper : IServiceResultErrorMapper<ThrowingCtorError, ProblemHttpResult>
{
  public ThrowingCtorMapper() => throw new InvalidOperationException("mapper must not be constructed");

  public ProblemHttpResult Map(ThrowingCtorError error) => throw new NotSupportedException();
}
