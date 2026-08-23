using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace CSharpFunctionalExtensions.HttpResults.Tests.Shared;

/// <summary>Builds an <see cref="HttpContext" /> whose request services are backed by a real service collection.</summary>
internal static class TestHttpContextHelper
{
  internal const string ProviderTitle = "Created by DI";
  internal const string ProviderType = "https://example.test/problems/from-di";

  /// <summary>Creates a request with a registered <see cref="RecordingResultProblemDetailsProvider" />.</summary>
  internal static TestHttpContext CreateWithProvider(Action<ProblemDetails>? additionalConfiguration = null)
  {
    var recorder = new RecordingResultProblemDetailsProvider(additionalConfiguration);
    var services = new ServiceCollection();
    services.AddSingleton<IResultProblemDetailsProvider>(recorder);

    return Create(services) with
    {
      Recorder = recorder,
    };
  }

  /// <summary>Creates a request without any registered <see cref="IResultProblemDetailsProvider" />.</summary>
  internal static TestHttpContext CreateWithoutProvider()
  {
    var services = new ServiceCollection();
    services.AddSingleton<ServiceMapperDependency>();

    return Create(services);
  }

  internal static TestHttpContext Create(IServiceCollection services)
  {
    var serviceProvider = services.BuildServiceProvider();
    return new TestHttpContext(new DefaultHttpContext { RequestServices = serviceProvider }, serviceProvider);
  }
}
