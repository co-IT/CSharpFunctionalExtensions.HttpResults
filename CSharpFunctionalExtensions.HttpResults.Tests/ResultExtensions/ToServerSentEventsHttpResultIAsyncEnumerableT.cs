#if NET10_0_OR_GREATER

using System.Net.Mime;
using CSharpFunctionalExtensions.HttpResults.ResultExtensions;
using CSharpFunctionalExtensions.HttpResults.Tests.Utils;
using AwesomeAssertions;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CSharpFunctionalExtensions.HttpResults.Tests.ResultExtensions;

public class ToServerSentEventsHttpResultIAsyncEnumerableT
{
  [Fact]
  public async Task ResultIAsyncEnumerableT_Success_can_be_mapped_to_ServerSentEventsHttpResult()
  {
    var testValues = new[] { 42, 420 };
    var eventType = "TestEvent";

    var asyncEnumerable = testValues.AsAsyncEnumerable();

    var result = Assert.IsType<ServerSentEventsResult<int>>(
      Result.Success(asyncEnumerable).ToServerSentEventsHttpResult(eventType).Result
    );

    var (response, values) = await result.ExecuteAndGetResponseAndValues();

    result.StatusCode.Should().Be(200);
    response.ContentType.Should().Be(MediaTypeNames.Text.EventStream);
    values.Select(v => v.Data).Should().Contain(testValues.Select(v => v.ToString()));
    values.Select(v => v.Event).Should().AllBe(eventType);
  }

  [Fact]
  public async Task ResultIAsyncEnumerableT_Success_can_be_mapped_to_ServerSentEventsHttpResult_Async()
  {
    var testValues = new[] { 42, 420 };
    var eventType = "TestEvent";

    var asyncEnumerable = testValues.AsAsyncEnumerable();

    var result = Assert.IsType<ServerSentEventsResult<int>>(
      (await Task.FromResult(Result.Success(asyncEnumerable)).ToServerSentEventsHttpResult(eventType)).Result
    );

    var (response, values) = await result.ExecuteAndGetResponseAndValues();

    result.StatusCode.Should().Be(200);
    response.ContentType.Should().Be(MediaTypeNames.Text.EventStream);
    values.Select(v => v.Data).Should().Contain(testValues.Select(v => v.ToString()));
    values.Select(v => v.Event).Should().AllBe(eventType);
  }

  [Fact]
  public void ResultIAsyncEnumerableT_Failure_can_be_mapped_to_ServerSentEventsHttpResult()
  {
    var error = "Error";

    var result = Assert.IsType<ProblemHttpResult>(
      Result.Failure<IAsyncEnumerable<int>>(error).ToServerSentEventsHttpResult().Result
    );

    result.StatusCode.Should().Be(400);
    result.ProblemDetails.Status.Should().Be(400);
    result.ProblemDetails.Detail.Should().Be(error);
  }

  [Fact]
  public async Task ResultIAsyncEnumerableT_Failure_can_be_mapped_to_ServerSentEventsHttpResult_Async()
  {
    var error = "Error";

    var result = Assert.IsType<ProblemHttpResult>(
      (await Task.FromResult(Result.Failure<IAsyncEnumerable<int>>(error)).ToServerSentEventsHttpResult()).Result
    );

    result.StatusCode.Should().Be(400);
    result.ProblemDetails.Status.Should().Be(400);
    result.ProblemDetails.Detail.Should().Be(error);
  }

  [Fact]
  public void ResultIAsyncEnumerableT_Failure_StatusCode_can_be_changed()
  {
    var statusCode = 418;
    var error = "Error";

    var result = Assert.IsType<ProblemHttpResult>(
      Result.Failure<IAsyncEnumerable<int>>(error).ToServerSentEventsHttpResult(failureStatusCode: statusCode).Result
    );

    result.StatusCode.Should().Be(statusCode);
    result.ProblemDetails.Status.Should().Be(statusCode);
    result.ProblemDetails.Detail.Should().Be(error);
  }

  [Fact]
  public async Task ResultIAsyncEnumerableT_Failure_StatusCode_can_be_changed_Async()
  {
    var statusCode = 418;
    var error = "Error";

    var result = Assert.IsType<ProblemHttpResult>(
      (
        await Task.FromResult(Result.Failure<IAsyncEnumerable<int>>(error))
          .ToServerSentEventsHttpResult(failureStatusCode: statusCode)
      ).Result
    );

    result.StatusCode.Should().Be(statusCode);
    result.ProblemDetails.Status.Should().Be(statusCode);
    result.ProblemDetails.Detail.Should().Be(error);
  }

  [Fact]
  public void ResultIAsyncEnumerableT_Failure_ProblemDetails_can_be_customized()
  {
    var error = "Error";
    var customTitle = "Custom Title";

    var result = Assert.IsType<ProblemHttpResult>(
      Result
        .Failure<IAsyncEnumerable<int>>(error)
        .ToServerSentEventsHttpResult(customizeProblemDetails: problemDetails => problemDetails.Title = customTitle)
        .Result
    );

    result.ProblemDetails.Title.Should().Be(customTitle);
  }

  [Fact]
  public async Task ResultIAsyncEnumerableT_Failure_ProblemDetails_can_be_customized_Async()
  {
    var error = "Error";
    var customTitle = "Custom Title";

    var result = Assert.IsType<ProblemHttpResult>(
      (
        await Task.FromResult(Result.Failure<IAsyncEnumerable<int>>(error))
          .ToServerSentEventsHttpResult(customizeProblemDetails: problemDetails => problemDetails.Title = customTitle)
      ).Result
    );

    result.ProblemDetails.Title.Should().Be(customTitle);
  }
}

#endif
