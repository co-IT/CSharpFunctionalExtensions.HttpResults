using AwesomeAssertions;
using CSharpFunctionalExtensions;
using CSharpFunctionalExtensions.HttpResults.ResultExtensions;
using CSharpFunctionalExtensions.HttpResults.Tests.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.DependencyInjection;

namespace CSharpFunctionalExtensions.HttpResults.Tests.ResultExtensions.ProblemDetailsProvider;

public class ToOkHttpResultT_Context
{
  [Fact]
  public void Failure_uses_the_registered_provider()
  {
    using var http = TestHttpContextHelper.CreateWithProvider();

    var result = Result.Failure<string>("Error").ToOkHttpResult(http).Result as ProblemHttpResult;

    result!.StatusCode.Should().Be(400);
    result.ProblemDetails.Title.Should().Be(TestHttpContextHelper.ProviderTitle);
    result.ProblemDetails.Type.Should().Be(TestHttpContextHelper.ProviderType);
    result.ProblemDetails.Detail.Should().Be("Error");
    http.Recorder!.ReceivedError.Should().Be("Error");
    http.Recorder!.ReceivedStatusCode.Should().Be(400);
    http.Recorder!.ReceivedContext.Should().BeSameAs(http.Context);
  }

  [Fact]
  public void Failure_without_registered_provider_fails_fast()
  {
    using var http = TestHttpContextHelper.CreateWithoutProvider();

    var act = () => Result.Failure<string>("Error").ToOkHttpResult(http);

    act.Should().Throw<InvalidOperationException>();
  }

  [Fact]
  public void Customize_callback_runs_after_the_provider()
  {
    const string customTitle = "Custom Title";
    using var http = TestHttpContextHelper.CreateWithProvider();

    var result =
      Result.Failure<string>("Error").ToOkHttpResult(http, customizeProblemDetails: p => p.Title = customTitle).Result
      as ProblemHttpResult;

    result!.ProblemDetails.Title.Should().Be(customTitle);
  }

  [Fact]
  public async Task Async_overload_forwards_the_context()
  {
    using var http = TestHttpContextHelper.CreateWithProvider();

    var problem =
      (await Task.FromResult(Result.Failure<string>("Error")).ToOkHttpResult(http)).Result as ProblemHttpResult;
    problem!.ProblemDetails.Title.Should().Be(TestHttpContextHelper.ProviderTitle);
    http.Recorder!.ReceivedContext.Should().BeSameAs(http.Context);
  }

  [Fact]
  public void Success_never_touches_request_services_even_with_a_throwing_provider()
  {
    var services = new ServiceCollection();
    services.AddSingleton<IResultProblemDetailsProvider>(_ =>
      throw new InvalidOperationException("provider must not be constructed")
    );
    using var http = TestHttpContextHelper.Create(services);

    var act = () => Result.Success<string>("ok").ToOkHttpResult(http);

    act.Should().NotThrow();
  }

  [Fact]
  public void Null_context_fails_fast_with_a_descriptive_exception()
  {
    var act = () => Result.Failure<string>("Error").ToOkHttpResult((HttpContext)null!);

    act.Should()
      .Throw<InvalidOperationException>()
      .WithMessage("A HttpContext is required to resolve the registered IResultProblemDetailsProvider.");
  }
}
