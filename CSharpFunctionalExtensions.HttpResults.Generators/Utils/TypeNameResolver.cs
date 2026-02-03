using Microsoft.CodeAnalysis;

namespace CSharpFunctionalExtensions.HttpResults.Generators.Utils;

internal static class TypeNameResolver
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
