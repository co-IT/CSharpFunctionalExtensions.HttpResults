using CSharpFunctionalExtensions.HttpResults.Generators.Analysis;
using Microsoft.CodeAnalysis;

namespace CSharpFunctionalExtensions.HttpResults.Generators.Discovery;

/// <summary>Discovers mapper and provider implementations once per compilation.</summary>
internal static class ImplementationScanner
{
  public static ImplementationScan Scan(Compilation compilation)
  {
    var resultMapperInterface = compilation.GetTypeByMetadataName(
      $"{MapperSymbolResolver.LibraryNamespace}.{MapperSymbolResolver.ResultErrorMapperInterfaceMetadataName}"
    );
    var serviceMapperInterface = compilation.GetTypeByMetadataName(
      $"{MapperSymbolResolver.LibraryNamespace}.{MapperSymbolResolver.ServiceResultErrorMapperInterfaceMetadataName}"
    );
    var providerInterface = compilation.GetTypeByMetadataName(
      $"{MapperSymbolResolver.LibraryNamespace}.IResultProblemDetailsProvider"
    );

    var sourceTypes = GetAllTypes(compilation.Assembly.GlobalNamespace).ToArray();
    var sourceMappers = resultMapperInterface is null
      ? []
      : sourceTypes.Where(type => ImplementsInterface(type, resultMapperInterface)).ToArray();

    var providers = providerInterface is null
      ? new List<INamedTypeSymbol>()
      : sourceTypes.Where(type => ImplementsInterface(type, providerInterface)).ToList();
    var referencedServiceMappers = new List<INamedTypeSymbol>();

    var libraryAssembly = providerInterface?.ContainingAssembly ?? resultMapperInterface?.ContainingAssembly;
    if (libraryAssembly is not null)
    {
      foreach (var reference in compilation.References)
      {
        if (compilation.GetAssemblyOrModuleSymbol(reference) is not IAssemblySymbol assembly)
          continue;

        if (SymbolEqualityComparer.Default.Equals(assembly, libraryAssembly))
          continue;

        // Only assemblies that reference this library can contain relevant implementations. This avoids
        // traversing every type in the framework and unrelated dependencies for each consumer compilation.
        if (!ReferencesAssembly(assembly, libraryAssembly))
          continue;

        foreach (var type in GetAllTypes(assembly.GlobalNamespace))
        {
          if (
            providerInterface is not null
            && ImplementsInterface(type, providerInterface)
            && SymbolAccessibility.IsReferenceable(type, compilation.Assembly)
          )
            providers.Add(type);

          if (
            serviceMapperInterface is not null
            && ImplementsInterface(type, serviceMapperInterface)
            && SymbolAccessibility.IsReferenceable(type, compilation.Assembly)
          )
            referencedServiceMappers.Add(type);
        }
      }
    }

    return new ImplementationScan(
      sourceMappers.Distinct(SymbolEqualityComparer.Default).OfType<INamedTypeSymbol>().ToArray(),
      providers.Distinct(SymbolEqualityComparer.Default).OfType<INamedTypeSymbol>().ToArray(),
      referencedServiceMappers.Distinct(SymbolEqualityComparer.Default).OfType<INamedTypeSymbol>().ToArray()
    );
  }

  private static bool ImplementsInterface(INamedTypeSymbol type, INamedTypeSymbol interfaceType) =>
    type.AllInterfaces.Any(implemented =>
      SymbolEqualityComparer.Default.Equals(implemented.OriginalDefinition, interfaceType)
    );

  private static bool ReferencesAssembly(IAssemblySymbol assembly, IAssemblySymbol referencedAssembly) =>
    assembly.Modules.Any(module =>
      module.ReferencedAssemblySymbols.Any(reference =>
        SymbolEqualityComparer.Default.Equals(reference, referencedAssembly)
      )
    );

  private static IEnumerable<INamedTypeSymbol> GetAllTypes(INamespaceSymbol namespaceSymbol)
  {
    foreach (var type in namespaceSymbol.GetTypeMembers())
    {
      yield return type;

      foreach (var nestedType in GetNestedTypes(type))
        yield return nestedType;
    }

    foreach (var childNamespace in namespaceSymbol.GetNamespaceMembers())
    foreach (var type in GetAllTypes(childNamespace))
      yield return type;
  }

  private static IEnumerable<INamedTypeSymbol> GetNestedTypes(INamedTypeSymbol type)
  {
    foreach (var nestedType in type.GetTypeMembers())
    {
      yield return nestedType;

      foreach (var descendant in GetNestedTypes(nestedType))
        yield return descendant;
    }
  }
}
