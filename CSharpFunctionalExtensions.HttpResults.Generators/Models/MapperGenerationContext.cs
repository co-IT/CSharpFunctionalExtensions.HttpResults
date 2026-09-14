namespace CSharpFunctionalExtensions.HttpResults.Generators.Models;

/// <summary>
///   Carries all information a method renderer needs to generate the extension methods
///   for a single discovered error mapper.
/// </summary>
internal readonly record struct MapperGenerationContext
{
  /// <summary>Unique generated cache member used for a standard mapper.</summary>
  public required string CacheMemberName { get; init; }

  /// <summary>Fully qualified name of the mapped error type.</summary>
  public required string ErrorType { get; init; }

  /// <summary>Fully qualified name of the HttpResult type returned by the mapper's Map method.</summary>
  public required string HttpResultType { get; init; }

  /// <summary>Indicates whether the mapper implements <see cref="CSharpFunctionalExtensions.HttpResults.IServiceResultErrorMapper{TError, THttpResult}" />.</summary>
  public required bool IsServiceMapper { get; init; }

  /// <summary>Fully qualified name of the mapper type; used for request-service resolution of service mappers.</summary>
  public required string MapperFullyQualifiedName { get; init; }

  /// <summary>Fully qualified closed mapper interface used to invoke Map through its contract.</summary>
  public required string MapperInterfaceType { get; init; }

  /// <summary>Indicates whether the mapper's Map method returns exactly <see cref="Microsoft.AspNetCore.Http.HttpResults.ProblemHttpResult" />.</summary>
  public required bool IsProblemHttpResult { get; init; }
}
