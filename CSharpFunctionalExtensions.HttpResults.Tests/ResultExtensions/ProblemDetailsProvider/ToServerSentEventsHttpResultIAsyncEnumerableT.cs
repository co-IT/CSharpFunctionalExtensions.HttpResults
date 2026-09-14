#if NET10_0_OR_GREATER

using AwesomeAssertions;
using CSharpFunctionalExtensions.HttpResults.ResultExtensions;
using CSharpFunctionalExtensions.HttpResults.Tests.Shared;
using CSharpFunctionalExtensions.HttpResults.Tests.Utils;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CSharpFunctionalExtensions.HttpResults.Tests.ResultExtensions.ProblemDetailsProvider;

public class ToServerSentEventsHttpResultIAsyncEnumerableT_Context
{
  [Fact]
  public async Task Success_returns_server_sent_events_without_resolving_the_provider()
  {
    using var http = TestHttpContextHelper.CreateWithoutProvider();
    var values = new[] { 1, 2 }.AsAsyncEnumerable();

    var result = Assert.IsType<ServerSentEventsResult<int>>(
      Result.Success(values).ToServerSentEventsHttpResult(http, "number").Result
    );
    var (_, events) = await result.ExecuteAndGetResponseAndValues();

    events.Select(item => item.Data).Should().Contain(["1", "2"]);
  }

  [Fact]
  public void Failure_uses_the_registered_provider()
  {
    using var http = TestHttpContextHelper.CreateWithProvider();

    var result = Assert.IsType<ProblemHttpResult>(
      Result.Failure<IAsyncEnumerable<int>>("Error").ToServerSentEventsHttpResult(http).Result
    );

    result.ProblemDetails.Title.Should().Be(TestHttpContextHelper.ProviderTitle);
    http.Recorder!.ReceivedContext.Should().BeSameAs(http.Context);
  }

  [Fact]
  public async Task Async_overload_forwards_context_and_parameters()
  {
    using var http = TestHttpContextHelper.CreateWithProvider();

    var result = Assert.IsType<ProblemHttpResult>(
      (
        await Task.FromResult(Result.Failure<IAsyncEnumerable<int>>("Error"))
          .ToServerSentEventsHttpResult(http, eventType: "number", failureStatusCode: 418)
      ).Result
    );

    result.StatusCode.Should().Be(418);
    http.Recorder!.ReceivedStatusCode.Should().Be(418);
  }

  [Fact]
  public void Customize_callback_runs_after_the_provider()
  {
    using var http = TestHttpContextHelper.CreateWithProvider();

    var result = Assert.IsType<ProblemHttpResult>(
      Result
        .Failure<IAsyncEnumerable<int>>("Error")
        .ToServerSentEventsHttpResult(http, customizeProblemDetails: details => details.Title = "custom")
        .Result
    );

    result.ProblemDetails.Title.Should().Be("custom");
  }
}

#endif
