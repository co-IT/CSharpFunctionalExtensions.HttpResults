namespace CSharpFunctionalExtensions.HttpResults.IntegrationTests.Features.Documents;

/// <summary>Injected into the service mapper to prove constructor injection through the real pipeline.</summary>
public sealed class DocumentationLinkProvider
{
  public string For(string topic) => $"https://docs.example.test/errors/{topic}";
}
