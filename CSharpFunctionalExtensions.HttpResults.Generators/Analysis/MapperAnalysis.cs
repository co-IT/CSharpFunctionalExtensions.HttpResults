using CSharpFunctionalExtensions.HttpResults.Generators.Utils;
using Microsoft.CodeAnalysis;

namespace CSharpFunctionalExtensions.HttpResults.Generators.Analysis;

/// <summary>Valid source mappers and diagnostics produced while analysing mapper symbols.</summary>
internal sealed record MapperAnalysis(IReadOnlyList<MapperInfo> Mappers, IReadOnlyList<Diagnostic> Diagnostics);
