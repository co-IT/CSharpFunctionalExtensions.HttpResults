using Microsoft.CodeAnalysis;

namespace CSharpFunctionalExtensions.HttpResults.Generators.Analysis;

/// <summary>The selected provider, if unambiguous, and provider diagnostics.</summary>
internal sealed record ProviderAnalysis(INamedTypeSymbol? Provider, IReadOnlyList<Diagnostic> Diagnostics);
