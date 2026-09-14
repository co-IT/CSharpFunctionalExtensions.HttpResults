using Microsoft.CodeAnalysis;

namespace CSharpFunctionalExtensions.HttpResults.Generators.Analysis;

internal static class SymbolTypeNameFormatter
{
  private static readonly SymbolDisplayFormat FullyQualifiedWithNullables =
    SymbolDisplayFormat.FullyQualifiedFormat.WithMiscellaneousOptions(
      SymbolDisplayFormat.FullyQualifiedFormat.MiscellaneousOptions
        | SymbolDisplayMiscellaneousOptions.IncludeNullableReferenceTypeModifier
    );

  public static string GetFullyQualifiedTypeName(ITypeSymbol typeSymbol)
  {
    return typeSymbol.ToDisplayString(FullyQualifiedWithNullables);
  }
}
