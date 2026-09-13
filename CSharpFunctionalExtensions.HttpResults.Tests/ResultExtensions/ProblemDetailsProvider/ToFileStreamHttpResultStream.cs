using System.IO;
using AwesomeAssertions;
using CSharpFunctionalExtensions;
using CSharpFunctionalExtensions.HttpResults.ResultExtensions;
using CSharpFunctionalExtensions.HttpResults.Tests.Shared;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CSharpFunctionalExtensions.HttpResults.Tests.ResultExtensions.ProblemDetailsProvider;

public class ToFileStreamHttpResultStream_Context
{
  [Fact]
  public void Failure_uses_the_registered_provider()
  {
    using var http = TestHttpContextHelper.CreateWithProvider();

    var result = Result.Failure<Stream>("Error").ToFileStreamHttpResult(http).Result as ProblemHttpResult;

    result!.ProblemDetails.Title.Should().Be(TestHttpContextHelper.ProviderTitle);
    http.Recorder!.ReceivedContext.Should().BeSameAs(http.Context);
  }

  [Fact]
  public void Failure_without_registered_provider_fails_fast()
  {
    using var http = TestHttpContextHelper.CreateWithoutProvider();

    var act = () => Result.Failure<Stream>("Error").ToFileStreamHttpResult(http);

    act.Should().Throw<InvalidOperationException>();
  }

  [Fact]
  public void Customize_callback_runs_after_the_provider()
  {
    using var http = TestHttpContextHelper.CreateWithProvider();

    var result =
      Result
        .Failure<Stream>("Error")
        .ToFileStreamHttpResult(http, customizeProblemDetails: p => p.Title = "Custom Title")
        .Result as ProblemHttpResult;

    result!.ProblemDetails.Title.Should().Be("Custom Title");
  }

  [Fact]
  public async Task Async_overload_forwards_the_context()
  {
    using var http = TestHttpContextHelper.CreateWithProvider();

    var result =
      (await Task.FromResult(Result.Failure<Stream>("Error")).ToFileStreamHttpResult(http)).Result as ProblemHttpResult;

    result!.ProblemDetails.Title.Should().Be(TestHttpContextHelper.ProviderTitle);
    http.Recorder!.ReceivedContext.Should().BeSameAs(http.Context);
  }
}
