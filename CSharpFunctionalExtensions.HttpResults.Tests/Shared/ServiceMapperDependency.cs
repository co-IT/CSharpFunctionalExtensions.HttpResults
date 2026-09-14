namespace CSharpFunctionalExtensions.HttpResults.Tests.Shared;

/// <summary>Injected dependency proving constructor injection works.</summary>
public sealed class ServiceMapperDependency
{
  public string Prefix { get; } = "resolved-by-di:";
}
