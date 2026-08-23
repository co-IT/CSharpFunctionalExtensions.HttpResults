using Xunit;

namespace CSharpFunctionalExtensions.HttpResults.Tests;

/// <summary>
///   Tests around <see cref="ProblemDetailsMappingProvider" /> mutate its static mappings and must
///   not run in parallel with each other or with tests reading those mappings.
/// </summary>
[CollectionDefinition(nameof(ProblemDetailsMappingProvider), DisableParallelization = true)]
public sealed class ProblemDetailsMappingProviderCollection { }
