using Microsoft.CodeAnalysis;

namespace CSharpFunctionalExtensions.HttpResults.Generators.Discovery;

/// <summary>Symbols relevant to mapping generation discovered in source and referenced assemblies.</summary>
internal sealed record ImplementationScan(
  IReadOnlyList<INamedTypeSymbol> SourceMappers,
  IReadOnlyList<INamedTypeSymbol> Providers,
  IReadOnlyList<INamedTypeSymbol> ReferencedServiceMappers
);
