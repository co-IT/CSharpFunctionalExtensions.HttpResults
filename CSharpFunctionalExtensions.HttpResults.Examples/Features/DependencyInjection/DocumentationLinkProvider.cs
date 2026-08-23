namespace CSharpFunctionalExtensions.HttpResults.Examples.Features.DependencyInjection;

/// <summary>Builds documentation links and is injected into the service mapper.</summary>
public sealed class DocumentationLinkProvider
{
  public string For(string topic) => $"https://docs.example.com/errors/{topic}";
}
