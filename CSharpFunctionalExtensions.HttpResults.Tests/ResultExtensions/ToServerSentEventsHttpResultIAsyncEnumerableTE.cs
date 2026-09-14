#if NET10_0_OR_GREATER

using System.Net.Mime;
using CSharpFunctionalExtensions.HttpResults.ResultExtensions;
using CSharpFunctionalExtensions.HttpResults.Tests.Shared;
using CSharpFunctionalExtensions.HttpResults.Tests.Utils;
using AwesomeAssertions;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CSharpFunctionalExtensions.HttpResults.Tests.ResultExtensions;

public class ToServerSentEventsHttpResultIAsyncEnumerableTE
{
  [Fact]
  public async Task ResultIAsyncEnumerableTE_Success_can_be_mapped_to_ServerSentEventsHttpResult()
  {
    var testValues = new[] { 42, 420 };
    var eventType = "TestEvent";

    var asyncEnumerable = testValues.AsAsyncEnumerable();

    var result = Assert.IsType<ServerSentEventsResult<int>>(
      Result
        .Success<IAsyncEnumerable<int>, DocumentMissingError>(asyncEnumerable)
        .ToServerSentEventsHttpResult(eventType)
        .Result
    );

    var (response, values) = await result.ExecuteAndGetResponseAndValues();

    result.StatusCode.Should().Be(200);
    response.ContentType.Should().Be(MediaTypeNames.Text.EventStream);
    values.Select(v => v.Data).Should().Contain(testValues.Select(v => v.ToString()));
    values.Select(v => v.Event).Should().AllBe(eventType);
  }

  [Fact]
  public async Task ResultIAsyncEnumerableTE_Success_can_be_mapped_to_ServerSentEventsHttpResult_Async()
  {
    var testValues = new[] { 42, 420 };
    var eventType = "TestEvent";

    var asyncEnumerable = testValues.AsAsyncEnumerable();

    var result = Assert.IsType<ServerSentEventsResult<int>>(
      (
        await Task.FromResult(Result.Success<IAsyncEnumerable<int>, DocumentMissingError>(asyncEnumerable))
          .ToServerSentEventsHttpResult(eventType)
      ).Result
    );

    var (response, values) = await result.ExecuteAndGetResponseAndValues();

    result.StatusCode.Should().Be(200);
    response.ContentType.Should().Be(MediaTypeNames.Text.EventStream);
    values.Select(v => v.Data).Should().Contain(testValues.Select(v => v.ToString()));
    values.Select(v => v.Event).Should().AllBe(eventType);
  }

  [Fact]
  public void ResultIAsyncEnumerableTE_Failure_can_be_mapped_to_ServerSentEventsHttpResult()
  {
    var error = new DocumentMissingError { DocumentId = Guid.NewGuid().ToString() };

    var result = Assert.IsType<NotFound<string>>(
      Result.Failure<IAsyncEnumerable<int>, DocumentMissingError>(error).ToServerSentEventsHttpResult().Result
    );

    result.StatusCode.Should().Be(404);
    result.Value.Should().Be(error.DocumentId);
  }

  [Fact]
  public async Task ResultIAsyncEnumerableTE_Failure_can_be_mapped_to_ServerSentEventsHttpResult_Async()
  {
    var error = new DocumentMissingError { DocumentId = Guid.NewGuid().ToString() };

    var result = Assert.IsType<NotFound<string>>(
      (
        await Task.FromResult(Result.Failure<IAsyncEnumerable<int>, DocumentMissingError>(error))
          .ToServerSentEventsHttpResult()
      ).Result
    );

    result.StatusCode.Should().Be(404);
    result.Value.Should().Be(error.DocumentId);
  }
}

#endif
