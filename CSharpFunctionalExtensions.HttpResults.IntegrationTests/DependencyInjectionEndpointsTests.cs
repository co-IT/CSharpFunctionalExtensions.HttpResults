using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using AwesomeAssertions;
using CSharpFunctionalExtensions.HttpResults.IntegrationTests.TestApp;

namespace CSharpFunctionalExtensions.HttpResults.IntegrationTests;

/// <summary>
///   End-to-end tests over a real request pipeline (TestServer): proves that service mappers and
///   the registered IResultProblemDetailsProvider are resolved from RequestServices at runtime,
///   and that customizeProblemDetails still runs last - the pieces unit tests can only simulate.
/// </summary>
public class DependencyInjectionEndpointsTests : IClassFixture<TestAppFactory>
{
  private readonly TestAppFactory _factory;

  public DependencyInjectionEndpointsTests(TestAppFactory factory) => _factory = factory;

  [Fact]
  public async Task Service_mapper_is_resolved_from_request_services_with_injected_dependency()
  {
    var client = _factory.CreateClient();

    var response = await client.GetAsync("/documents/doc-42", TestContext.Current.CancellationToken);
    var problem = await ReadProblemDetailsAsync(response);

    response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    problem.GetProperty("title").GetString().Should().Be("Document not found");
    problem.GetProperty("type").GetString().Should().Be("https://docs.example.test/errors/document-not-found");
    problem.GetProperty("detail").GetString().Should().Be("Document doc-42 could not be found.");
  }

  [Fact]
  public async Task Built_in_string_error_uses_the_registered_provider_and_the_callback_runs_last()
  {
    var client = _factory.CreateClient();

    var response = await client.GetAsync("/books/7", TestContext.Current.CancellationToken);
    var problem = await ReadProblemDetailsAsync(response);

    response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    problem.GetProperty("status").GetInt32().Should().Be(404);
    problem
      .GetProperty("title")
      .GetString()
      .Should()
      .Be("Not Found", "the MVC ProblemDetailsFactory supplies the RFC9457 title");
    problem.GetProperty("detail").GetString().Should().Be("Book 7 not found");
    problem
      .GetProperty("source")
      .GetString()
      .Should()
      .Be("mvc-problemdetails-factory", "the registered provider created the instance");
    problem
      .GetProperty("callback")
      .GetString()
      .Should()
      .Be("ran-last", "customizeProblemDetails must be applied after provider creation");
  }

  [Fact]
  public async Task Static_zero_config_mapping_still_works_without_a_context()
  {
    var client = _factory.CreateClient();

    var response = await client.GetAsync("/static/books/7", TestContext.Current.CancellationToken);
    var problem = await ReadProblemDetailsAsync(response);

    response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    problem.GetProperty("status").GetInt32().Should().Be(404);
    problem.GetProperty("title").GetString().Should().Be("Not Found");
    problem.TryGetProperty("source", out _).Should().BeFalse("no provider is involved on static paths");
  }

  [Fact]
  public async Task Success_arm_is_served_without_touching_request_services()
  {
    var client = _factory.CreateClient();

    var response = await client.GetAsync("/books", TestContext.Current.CancellationToken);

    response.IsSuccessStatusCode.Should().BeTrue();
    (await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken)).Should().Contain("all-books");
  }

  private static Task<JsonElement> ReadProblemDetailsAsync(HttpResponseMessage response) =>
    response.Content.ReadFromJsonAsync<JsonElement>(TestContext.Current.CancellationToken);
}
