using CSharpFunctionalExtensions.HttpResults.Generators.Utils;
using Microsoft.CodeAnalysis;

namespace CSharpFunctionalExtensions.HttpResults.Generators.Analysis;

/// <summary>Validates discovered provider implementations and selects the sole valid provider.</summary>
internal static class ProviderAnalyzer
{
  public static ProviderAnalysis Analyze(Compilation compilation, IReadOnlyList<INamedTypeSymbol> candidates)
  {
    var diagnostics = new List<Diagnostic>();
    var valid = new List<INamedTypeSymbol>();

    foreach (var candidate in candidates.OrderBy(TypeNameResolver.GetFullyQualifiedTypeName, StringComparer.Ordinal))
    {
      string? reason = null;
      if (candidate.TypeKind != TypeKind.Class || candidate.IsAbstract || candidate.IsStatic)
        reason = "the type must be a concrete, non-static class";
      else if (!SymbolAccessibility.IsReferenceable(candidate, compilation.Assembly))
        reason = "the type must be closed and accessible to generated code";
      else if (!SymbolAccessibility.HasPublicInstanceConstructor(candidate))
        reason = "a public constructor is required for DI";

      if (reason is not null)
      {
        diagnostics.Add(
          Diagnostic.Create(GeneratorDiagnostics.UnsupportedProvider, GetLocation(candidate), candidate.Name, reason)
        );
        continue;
      }

      valid.Add(candidate);
    }

    if (valid.Count > 1)
    {
      diagnostics.Add(
        Diagnostic.Create(
          GeneratorDiagnostics.DuplicateProvider,
          GetLocation(valid[1]),
          string.Join(", ", valid.Select(TypeNameResolver.GetFullyQualifiedTypeName))
        )
      );
      return new ProviderAnalysis(null, diagnostics);
    }

    return new ProviderAnalysis(valid.SingleOrDefault(), diagnostics);
  }

  private static Location GetLocation(INamedTypeSymbol symbol) =>
    symbol.Locations.FirstOrDefault(location => location.IsInSource) ?? Location.None;
}
