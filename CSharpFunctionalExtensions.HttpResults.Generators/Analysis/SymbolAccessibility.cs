using Microsoft.CodeAnalysis;

namespace CSharpFunctionalExtensions.HttpResults.Generators.Analysis;

/// <summary>Accessibility and constructibility checks shared by mapper and provider analysis.</summary>
internal static class SymbolAccessibility
{
  public static bool IsReferenceable(INamedTypeSymbol type, IAssemblySymbol generatedAssembly)
  {
    for (var current = type; current is not null; current = current.ContainingType)
    {
      if (current.Arity != 0)
        return false;

      var sameAssembly = SymbolEqualityComparer.Default.Equals(current.ContainingAssembly, generatedAssembly);
      var validAccessibility = sameAssembly
        ? current.DeclaredAccessibility is Accessibility.Public or Accessibility.Internal
        : current.DeclaredAccessibility == Accessibility.Public;

      if (!validAccessibility)
        return false;
    }

    return true;
  }

  public static bool HasPublicInstanceConstructor(INamedTypeSymbol type) =>
    type.InstanceConstructors.Any(constructor => constructor.DeclaredAccessibility == Accessibility.Public);

  public static bool HasAccessibleParameterlessConstructor(INamedTypeSymbol type, IAssemblySymbol generatedAssembly) =>
    type.InstanceConstructors.Any(constructor =>
      constructor.Parameters.Length == 0 && IsAccessible(constructor.DeclaredAccessibility, type, generatedAssembly)
    );

  public static bool HasUnsatisfiedRequiredMembers(INamedTypeSymbol type, IMethodSymbol constructor)
  {
    if (
      constructor
        .GetAttributes()
        .Any(attribute =>
          attribute.AttributeClass?.ToDisplayString() == "System.Diagnostics.CodeAnalysis.SetsRequiredMembersAttribute"
        )
    )
      return false;

    for (var current = type; current is not null; current = current.BaseType)
    {
      if (
        current
          .GetMembers()
          .Any(member =>
            member
              is IPropertySymbol { IsStatic: false, IsRequired: true }
                or IFieldSymbol { IsStatic: false, IsRequired: true }
          )
      )
        return true;
    }

    return false;
  }

  private static bool IsAccessible(
    Accessibility accessibility,
    INamedTypeSymbol type,
    IAssemblySymbol generatedAssembly
  )
  {
    if (accessibility == Accessibility.Public)
      return true;

    return accessibility == Accessibility.Internal
      && SymbolEqualityComparer.Default.Equals(type.ContainingAssembly, generatedAssembly);
  }
}
