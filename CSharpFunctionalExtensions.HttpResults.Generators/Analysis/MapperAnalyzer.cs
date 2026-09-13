using CSharpFunctionalExtensions.HttpResults.Generators.Models;
using Microsoft.CodeAnalysis;

namespace CSharpFunctionalExtensions.HttpResults.Generators.Analysis;

/// <summary>Validates source mapper symbols and creates generation metadata for valid entries only.</summary>
internal static class MapperAnalyzer
{
  private const string ProblemHttpResultMetadataName = "Microsoft.AspNetCore.Http.HttpResults.ProblemHttpResult";

  public static MapperAnalysis Analyze(Compilation compilation, IReadOnlyList<INamedTypeSymbol> candidates)
  {
    var diagnostics = new List<Diagnostic>();
    var validCandidates = new List<(INamedTypeSymbol Symbol, INamedTypeSymbol MapperInterface)>();

    foreach (
      var candidate in candidates.OrderBy(SymbolTypeNameFormatter.GetFullyQualifiedTypeName, StringComparer.Ordinal)
    )
    {
      var mapperInterfaces = MapperSymbolResolver.GetClosedErrorMapperInterfaces(candidate).ToArray();
      if (mapperInterfaces.Length != 1)
      {
        diagnostics.Add(CreateUnsupported(candidate, "exactly one closed IResultErrorMapper<,> is required"));
        continue;
      }

      if (candidate.TypeKind != TypeKind.Class || candidate.IsAbstract || candidate.IsStatic)
      {
        diagnostics.Add(CreateUnsupported(candidate, "the type must be a concrete, non-static class"));
        continue;
      }

      if (!SymbolAccessibility.IsReferenceable(candidate, compilation.Assembly))
      {
        diagnostics.Add(CreateUnsupported(candidate, "the type must be closed and accessible to generated code"));
        continue;
      }

      if (MapperSymbolResolver.IsServiceMapper(candidate))
      {
        if (!SymbolAccessibility.HasPublicInstanceConstructor(candidate))
        {
          diagnostics.Add(CreateUnsupported(candidate, "a service mapper must have a public constructor for DI"));
          continue;
        }
      }
      else
      {
        var constructor = candidate.InstanceConstructors.FirstOrDefault(constructor =>
          constructor.Parameters.Length == 0
          && constructor.DeclaredAccessibility is Accessibility.Public or Accessibility.Internal
        );
        if (
          constructor is null
          || !SymbolAccessibility.HasAccessibleParameterlessConstructor(candidate, compilation.Assembly)
          || SymbolAccessibility.HasUnsatisfiedRequiredMembers(candidate, constructor)
        )
        {
          diagnostics.Add(
            Diagnostic.Create(
              GeneratorDiagnostics.MissingParameterlessConstructor,
              GetLocation(candidate),
              candidate.Name
            )
          );
          continue;
        }
      }

      validCandidates.Add((candidate, mapperInterfaces[0]));
    }

    var duplicateSymbols = new HashSet<INamedTypeSymbol>(SymbolEqualityComparer.Default);
    foreach (
      var group in validCandidates.GroupBy(
        entry => entry.MapperInterface.TypeArguments[0],
        SymbolEqualityComparer.Default
      )
    )
    {
      if (group.Count() <= 1)
        continue;

      var entries = group.ToArray();
      foreach (var entry in entries)
        duplicateSymbols.Add(entry.Symbol);

      diagnostics.Add(
        Diagnostic.Create(
          GeneratorDiagnostics.DuplicateMapper,
          GetLocation(entries[1].Symbol),
          entries[0].MapperInterface.TypeArguments[0].ToDisplayString()
        )
      );
    }

    var problemHttpResult = compilation.GetTypeByMetadataName(ProblemHttpResultMetadataName);
    var mapperDescriptors = validCandidates
      .Where(entry => !duplicateSymbols.Contains(entry.Symbol))
      .Select(entry => new MapperDescriptor
      {
        Symbol = entry.Symbol,
        CacheMemberName = "",
        FullyQualifiedName = SymbolTypeNameFormatter.GetFullyQualifiedTypeName(entry.Symbol),
        ErrorType = SymbolTypeNameFormatter.GetFullyQualifiedTypeName(entry.MapperInterface.TypeArguments[0]),
        HttpResultType = SymbolTypeNameFormatter.GetFullyQualifiedTypeName(entry.MapperInterface.TypeArguments[1]),
        MapperInterfaceType = SymbolTypeNameFormatter.GetFullyQualifiedTypeName(entry.MapperInterface),
        IsServiceMapper = MapperSymbolResolver.IsServiceMapper(entry.Symbol),
        IsProblemHttpResult =
          problemHttpResult is not null
          && SymbolEqualityComparer.Default.Equals(
            entry.MapperInterface.TypeArguments[1].OriginalDefinition,
            problemHttpResult
          ),
      })
      .OrderBy(info => info.FullyQualifiedName, StringComparer.Ordinal)
      .Select(
        (info, index) =>
          new MapperDescriptor
          {
            Symbol = info.Symbol,
            CacheMemberName = $"Mapper{index}",
            FullyQualifiedName = info.FullyQualifiedName,
            ErrorType = info.ErrorType,
            HttpResultType = info.HttpResultType,
            MapperInterfaceType = info.MapperInterfaceType,
            IsServiceMapper = info.IsServiceMapper,
            IsProblemHttpResult = info.IsProblemHttpResult,
          }
      )
      .ToArray();

    return new MapperAnalysis(mapperDescriptors, diagnostics);
  }

  private static Diagnostic CreateUnsupported(INamedTypeSymbol symbol, string reason) =>
    Diagnostic.Create(GeneratorDiagnostics.UnsupportedMapper, GetLocation(symbol), symbol.Name, reason);

  private static Location GetLocation(INamedTypeSymbol symbol) =>
    symbol.Locations.FirstOrDefault(location => location.IsInSource) ?? Location.None;
}
