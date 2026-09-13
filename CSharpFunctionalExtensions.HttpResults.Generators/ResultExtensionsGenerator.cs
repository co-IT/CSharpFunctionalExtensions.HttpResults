using System.Text;
using CSharpFunctionalExtensions.HttpResults.Generators.Analysis;
using CSharpFunctionalExtensions.HttpResults.Generators.Builders;
using CSharpFunctionalExtensions.HttpResults.Generators.Discovery;
using CSharpFunctionalExtensions.HttpResults.Generators.Emitters;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace CSharpFunctionalExtensions.HttpResults.Generators;

/// <summary>
///   Generates HTTP-result mapping extensions and zero-configuration DI registrations from the symbols
///   available to a consumer compilation.
/// </summary>
[Generator]
internal sealed class ResultExtensionsGenerator : IIncrementalGenerator
{
  public void Initialize(IncrementalGeneratorInitializationContext context)
  {
    context.RegisterSourceOutput(
      context.CompilationProvider,
      static (productionContext, compilation) =>
      {
        var implementations = ImplementationScanner.Scan(compilation);
        var mapperAnalysis = MapperAnalyzer.Analyze(compilation, implementations.SourceMappers);
        var providerAnalysis = ProviderAnalyzer.Analyze(compilation, implementations.Providers);

        foreach (var diagnostic in mapperAnalysis.Diagnostics.Concat(providerAnalysis.Diagnostics))
          productionContext.ReportDiagnostic(diagnostic);

        AddSource(
          productionContext,
          ResultErrorMapperCacheEmitter.HintName,
          ResultErrorMapperCacheEmitter.Emit(mapperAnalysis.Mappers)
        );
        AddSource(
          productionContext,
          "StringErrorResultExtensions.g.cs",
          new StringErrorResultExtensionsClassBuilder(providerAnalysis.Provider is not null).Build()
        );
        AddSource(
          productionContext,
          "ResultExtensions.g.cs",
          new ResultExtensionsClassBuilder(mapperAnalysis.Mappers).Build()
        );
        AddSource(
          productionContext,
          "UnitResultExtensions.g.cs",
          new UnitResultExtensionsClassBuilder(mapperAnalysis.Mappers).Build()
        );
        AddSource(
          productionContext,
          ServiceCollectionExtensionsEmitter.HintName,
          ServiceCollectionExtensionsEmitter.Emit(
            compilation,
            providerAnalysis.Provider,
            implementations.ReferencedServiceMappers,
            mapperAnalysis.Mappers
          )
        );
      }
    );
  }

  private static void AddSource(SourceProductionContext context, string hintName, string source) =>
    context.AddSource(hintName, SourceText.From(source, Encoding.UTF8));
}
