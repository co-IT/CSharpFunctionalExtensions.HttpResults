using Microsoft.CodeAnalysis;

namespace CSharpFunctionalExtensions.HttpResults.Generators.Analysis;

internal static class MapperSymbolResolver
{
  public const string ResultErrorMapperInterfaceMetadataName = "IResultErrorMapper`2";
  public const string ServiceResultErrorMapperInterfaceMetadataName = "IServiceResultErrorMapper`2";
  public const string LibraryNamespace = "CSharpFunctionalExtensions.HttpResults";

  /// <summary>
  ///   Checks whether the mapper implements the library's
  ///   <see cref="CSharpFunctionalExtensions.HttpResults.IServiceResultErrorMapper{TError, THttpResult}" />.
  ///   Must be checked before the standard interface because it inherits it.
  /// </summary>
  public static bool IsServiceMapper(INamedTypeSymbol? mapperSymbol) =>
    mapperSymbol?.AllInterfaces.Any(interfaceSymbol =>
      interfaceSymbol.OriginalDefinition.MetadataName == ServiceResultErrorMapperInterfaceMetadataName
      && interfaceSymbol.ContainingNamespace.ToDisplayString() == LibraryNamespace
    ) == true;

  /// <summary>
  ///   Returns the closed <c>IResultErrorMapper&lt;,&gt;</c> interfaces implemented by the mapper, resolved
  ///   from its type symbol. Service mappers qualify through inheritance, so explicit or inherited
  ///   <c>Map</c> implementations are covered without any syntactic lookup.
  /// </summary>
  public static IEnumerable<INamedTypeSymbol> GetClosedErrorMapperInterfaces(INamedTypeSymbol? mapperSymbol) =>
    mapperSymbol?.AllInterfaces.Where(interfaceSymbol =>
      interfaceSymbol.OriginalDefinition.MetadataName == ResultErrorMapperInterfaceMetadataName
      && interfaceSymbol.ContainingNamespace.ToDisplayString() == LibraryNamespace
    )
    ?? [];
}
