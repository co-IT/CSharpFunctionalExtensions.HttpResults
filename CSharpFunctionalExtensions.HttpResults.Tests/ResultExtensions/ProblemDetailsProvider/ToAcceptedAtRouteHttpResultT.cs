using AwesomeAssertions;
using CSharpFunctionalExtensions;
using CSharpFunctionalExtensions.HttpResults.ResultExtensions;
using CSharpFunctionalExtensions.HttpResults.Tests.Shared;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CSharpFunctionalExtensions.HttpResults.Tests.ResultExtensions.ProblemDetailsProvider;

public class ToAcceptedAtRouteHttpResultT_Context
{
  [Fact]
  public void Failure_uses_the_registered_provider()
  {
    using var http = TestHttpContextHelper.CreateWithProvider();

    var result = Result.Failure<string>("Error").ToAcceptedAtRouteHttpResult(http).Result as ProblemHttpResult;

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

    var act = () => Result.Failure<string>("Error").ToAcceptedAtRouteHttpResult(http);

    act.Should().Throw<InvalidOperationException>();
  }

  [Fact]
  public void Customize_callback_runs_after_the_provider()
  {
    const string customTitle = "Custom Title";
    using var http = TestHttpContextHelper.CreateWithProvider();

    var result =
      Result
        .Failure<string>("Error")
        .ToAcceptedAtRouteHttpResult(http, customizeProblemDetails: p => p.Title = customTitle)
        .Result as ProblemHttpResult;

    result!.ProblemDetails.Title.Should().Be(customTitle);
  }

  [Fact]
  public async Task Async_overload_forwards_the_context()
  {
    using var http = TestHttpContextHelper.CreateWithProvider();

    var problem =
      (await Task.FromResult(Result.Failure<string>("Error")).ToAcceptedAtRouteHttpResult(http)).Result
      as ProblemHttpResult;
    problem!.ProblemDetails.Title.Should().Be(TestHttpContextHelper.ProviderTitle);
    http.Recorder!.ReceivedContext.Should().BeSameAs(http.Context);
  }
}
