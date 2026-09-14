using Microsoft.CodeAnalysis;

namespace CSharpFunctionalExtensions.HttpResults.Generators.Models;

/// <summary>
///   Precomputed information about one discovered error mapper, shared by analysis,
///   the builders and the registration emitter.
/// </summary>
internal sealed class MapperDescriptor
{
  /// <summary>Declared symbol of the mapper class.</summary>
  public required INamedTypeSymbol Symbol { get; init; }

  /// <summary>Unique generated cache member for non-service mappers.</summary>
  public required string CacheMemberName { get; init; }

  /// <summary>Fully qualified name of the mapper type.</summary>
  public required string FullyQualifiedName { get; init; }

  /// <summary>Fully qualified name of the mapped error type (first type argument of IResultErrorMapper&lt;,&gt;).</summary>
  public required string ErrorType { get; init; }

  /// <summary>Fully qualified name of the HttpResult type (second type argument of IResultErrorMapper&lt;,&gt;).</summary>
  public required string HttpResultType { get; init; }

  /// <summary>Fully qualified closed IResultErrorMapper interface implemented by the mapper.</summary>
  public required string MapperInterfaceType { get; init; }

  public required bool IsServiceMapper { get; init; }

  /// <summary>Indicates whether the Map method returns exactly <see cref="Microsoft.AspNetCore.Http.HttpResults.ProblemHttpResult" />.</summary>
  public required bool IsProblemHttpResult { get; init; }

  public MapperGenerationContext ToContext() =>
    new()
    {
      CacheMemberName = CacheMemberName,
      ErrorType = ErrorType,
      HttpResultType = HttpResultType,
      IsServiceMapper = IsServiceMapper,
      MapperFullyQualifiedName = FullyQualifiedName,
      MapperInterfaceType = MapperInterfaceType,
      IsProblemHttpResult = IsProblemHttpResult,
    };
}
