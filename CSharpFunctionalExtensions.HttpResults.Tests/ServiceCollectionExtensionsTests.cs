using AwesomeAssertions;
using CSharpFunctionalExtensions.HttpResults.Tests.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace CSharpFunctionalExtensions.HttpResults.Tests;

public class ServiceCollectionExtensionsTests
{
  [Fact]
  public void Registers_the_discovered_provider_as_scoped()
  {
    var services = new ServiceCollection();
    services.AddCSharpFunctionalExtensionsHttpResults();

    using var provider = services.BuildServiceProvider(new ServiceProviderOptions { ValidateScopes = true });
    var rootResolution = () => provider.GetRequiredService<IResultProblemDetailsProvider>();
    rootResolution.Should().Throw<InvalidOperationException>();

    using var firstScope = provider.CreateScope();
    using var secondScope = provider.CreateScope();
    var first = firstScope.ServiceProvider.GetRequiredService<IResultProblemDetailsProvider>();
    first.Should().BeOfType<RecordingResultProblemDetailsProvider>();
    firstScope.ServiceProvider.GetRequiredService<IResultProblemDetailsProvider>().Should().BeSameAs(first);
    secondScope.ServiceProvider.GetRequiredService<IResultProblemDetailsProvider>().Should().NotBeSameAs(first);
  }

  [Fact]
  public void A_pre_registered_provider_wins_regardless_of_call_order()
  {
    var custom = new RecordingResultProblemDetailsProvider();

    var services = new ServiceCollection();
    services.AddSingleton<IResultProblemDetailsProvider>(custom);
    services.AddCSharpFunctionalExtensionsHttpResults();

    using var provider = services.BuildServiceProvider();
    provider.GetRequiredService<IResultProblemDetailsProvider>().Should().BeSameAs(custom);
  }

  [Fact]
  public void A_provider_registered_after_the_generated_registration_wins()
  {
    var custom = new RecordingResultProblemDetailsProvider();
    var services = new ServiceCollection();
    services.AddCSharpFunctionalExtensionsHttpResults();
    services.AddSingleton<IResultProblemDetailsProvider>(custom);

    using var provider = services.BuildServiceProvider();

    provider.GetRequiredService<IResultProblemDetailsProvider>().Should().BeSameAs(custom);
  }

  [Fact]
  public void Is_idempotent_when_called_twice()
  {
    var services = new ServiceCollection();
    services.AddCSharpFunctionalExtensionsHttpResults();
    services.AddCSharpFunctionalExtensionsHttpResults();

    using var provider = services.BuildServiceProvider();
    using var scope = provider.CreateScope();
    scope.ServiceProvider.GetServices<IResultProblemDetailsProvider>().Should().HaveCount(1);
  }

  [Fact]
  public void Discovered_service_mappers_are_registered_scoped_with_dependencies()
  {
    var services = new ServiceCollection();
    services.AddSingleton<ServiceMapperDependency>();
    services.AddCSharpFunctionalExtensionsHttpResults();

    using var provider = services.BuildServiceProvider();
    using var firstScope = provider.CreateScope();
    using var secondScope = provider.CreateScope();
    var mapper = firstScope.ServiceProvider.GetRequiredService<ServiceDocumentErrorMapper>();
    mapper.Map(new ServiceDocumentError { DocumentId = "x" }).ProblemDetails.Detail.Should().Be("resolved-by-di:x");
    firstScope.ServiceProvider.GetRequiredService<ServiceDocumentErrorMapper>().Should().BeSameAs(mapper);
    secondScope.ServiceProvider.GetRequiredService<ServiceDocumentErrorMapper>().Should().NotBeSameAs(mapper);
  }

  [Theory]
  [InlineData(true)]
  [InlineData(false)]
  public void Explicit_service_mapper_registration_can_override_the_generated_default(bool registerBeforeHelper)
  {
    var custom = new ServiceDocumentErrorMapper(new ServiceMapperDependency());
    var services = new ServiceCollection();
    if (registerBeforeHelper)
      services.AddSingleton(custom);

    services.AddCSharpFunctionalExtensionsHttpResults();

    if (!registerBeforeHelper)
      services.AddSingleton(custom);

    using var provider = services.BuildServiceProvider();

    provider.GetRequiredService<ServiceDocumentErrorMapper>().Should().BeSameAs(custom);
  }
}
