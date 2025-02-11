using FluentAssertions;

namespace CSharpFunctionalExtensions.HttpResults.Tests;

public class ProblemDetailsMappingProviderTests
{
  public ProblemDetailsMappingProviderTests()
  {
    ProblemDetailsMappingProvider.ResetToDefaults(); //restore initial state in static class for each run
  }

  [Fact]
  public void Can_find_title_and_type_for_status_code()
  {
    var problemDetails = ProblemDetailsMappingProvider.FindMapping(400);

    problemDetails.Title.Should().NotBeEmpty();
    problemDetails.Type.Should().NotBeEmpty();
  }

  [Fact]
  public void Returns_null_for_unknown_status_code()
  {
    var problemDetails = ProblemDetailsMappingProvider.FindMapping(888);

    problemDetails.Title.Should().BeNull();
    problemDetails.Type.Should().BeNull();
  }

  [Fact]
  public void Can_get_default_mappings()
  {
    ProblemDetailsMappingProvider.DefaultMappings.Should().NotBeEmpty();
  }

  [Fact]
  public void Can_set_default_mappings()
  {
    ProblemDetailsMappingProvider.DefaultMappings = new Dictionary<int, (string? Title, string? Type)>();
    ProblemDetailsMappingProvider.DefaultMappings.Should().BeEmpty();
  }

  [Fact]
  public void Can_reset_default_mappings()
  {
    ProblemDetailsMappingProvider.DefaultMappings = new Dictionary<int, (string? Title, string? Type)>();
    ProblemDetailsMappingProvider.ResetToDefaults();
    ProblemDetailsMappingProvider.DefaultMappings.Should().NotBeEmpty();
  }

  [Fact]
  public void Can_add_custom_mapping()
  {
    var statusCode = 777;
    var title = "Foo";
    var type = "Bar";

    ProblemDetailsMappingProvider.AddOrUpdateMapping(statusCode, title, type);
    var mapping = ProblemDetailsMappingProvider.FindMapping(statusCode);
    mapping.Title.Should().Be(title);
    mapping.Type.Should().Be(type);
  }

  [Fact]
  public void Can_update_custom_mapping()
  {
    var statusCode = 400;
    var title = "Foo";
    var type = "Bar";

    var mapping = ProblemDetailsMappingProvider.FindMapping(statusCode);
    mapping.Title.Should().NotBe(title);
    mapping.Type.Should().NotBe(type);

    ProblemDetailsMappingProvider.AddOrUpdateMapping(statusCode, title, type);

    mapping = ProblemDetailsMappingProvider.FindMapping(statusCode);
    mapping.Title.Should().Be(title);
    mapping.Type.Should().Be(type);
  }
}
