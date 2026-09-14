using Microsoft.CodeAnalysis;

namespace CSharpFunctionalExtensions.HttpResults.Generators.Analysis;

/// <summary>All diagnostics emitted while analysing mapping implementations.</summary>
internal static class GeneratorDiagnostics
{
  public static readonly DiagnosticDescriptor DuplicateMapper = new(
    "CFEHTTPR002",
    "Duplicate ResultErrorMapper",
    "Error type '{0}' has multiple IResultErrorMapper or IServiceResultErrorMapper implementations",
    "Mapping",
    DiagnosticSeverity.Error,
    true
  );

  public static readonly DiagnosticDescriptor MissingParameterlessConstructor = new(
    "CFEHTTPR004",
    "Missing parameterless constructor in IResultErrorMapper",
    "Class '{0}' does not have an accessible parameterless constructor without unsatisfied required members. Implement IServiceResultErrorMapper<,> to use dependency injection instead.",
    "Mapping",
    DiagnosticSeverity.Error,
    true
  );

  public static readonly DiagnosticDescriptor DuplicateProvider = new(
    "CFEHTTPR005",
    "Multiple result problem-details providers",
    "Multiple IResultProblemDetailsProvider implementations were found: {0}",
    "Mapping",
    DiagnosticSeverity.Error,
    true
  );

  public static readonly DiagnosticDescriptor UnsupportedMapper = new(
    "CFEHTTPR006",
    "Unsupported result error mapper",
    "Mapper class '{0}' is unsupported: {1}",
    "Mapping",
    DiagnosticSeverity.Error,
    true
  );

  public static readonly DiagnosticDescriptor UnsupportedProvider = new(
    "CFEHTTPR007",
    "Unsupported result problem-details provider",
    "Provider class '{0}' is unsupported: {1}",
    "Mapping",
    DiagnosticSeverity.Error,
    true
  );
}
