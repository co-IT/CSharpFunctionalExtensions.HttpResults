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

    var result =
      Result.Success(values).ToServerSentEventsHttpResult(http, "number").Result as ServerSentEventsResult<int>;
    var (_, events) = await result!.ExecuteAndGetResponseAndValues();

    events.Select(item => item.Data).Should().Contain(["1", "2"]);
  }

  [Fact]
  public void Failure_uses_the_registered_provider()
  {
    using var http = TestHttpContextHelper.CreateWithProvider();

    var result =
      Result.Failure<IAsyncEnumerable<int>>("Error").ToServerSentEventsHttpResult(http).Result as ProblemHttpResult;

    result!.ProblemDetails.Title.Should().Be(TestHttpContextHelper.ProviderTitle);
    http.Recorder!.ReceivedContext.Should().BeSameAs(http.Context);
  }

  [Fact]
  public async Task Async_overload_forwards_context_and_parameters()
  {
    using var http = TestHttpContextHelper.CreateWithProvider();

    var result =
      (
        await Task.FromResult(Result.Failure<IAsyncEnumerable<int>>("Error"))
          .ToServerSentEventsHttpResult(http, eventType: "number", failureStatusCode: 418)
      ).Result as ProblemHttpResult;

    result!.StatusCode.Should().Be(418);
    http.Recorder!.ReceivedStatusCode.Should().Be(418);
  }

  [Fact]
  public void Customize_callback_runs_after_the_provider()
  {
    using var http = TestHttpContextHelper.CreateWithProvider();

    var result =
      Result
        .Failure<IAsyncEnumerable<int>>("Error")
        .ToServerSentEventsHttpResult(http, customizeProblemDetails: details => details.Title = "custom")
        .Result as ProblemHttpResult;

    result!.ProblemDetails.Title.Should().Be("custom");
  }
}

#endif
