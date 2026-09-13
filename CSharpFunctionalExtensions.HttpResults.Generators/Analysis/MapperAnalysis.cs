using CSharpFunctionalExtensions.HttpResults.Generators.Models;
using Microsoft.CodeAnalysis;

namespace CSharpFunctionalExtensions.HttpResults.Generators.Analysis;

/// <summary>Valid source mappers and diagnostics produced while analysing mapper symbols.</summary>
internal sealed record MapperAnalysis(IReadOnlyList<MapperDescriptor> Mappers, IReadOnlyList<Diagnostic> Diagnostics);
