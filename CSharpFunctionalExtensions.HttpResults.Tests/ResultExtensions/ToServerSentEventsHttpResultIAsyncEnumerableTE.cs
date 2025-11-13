#if NET10_0_OR_GREATER

using System.Net.Mime;
using CSharpFunctionalExtensions.HttpResults.ResultExtensions;
using CSharpFunctionalExtensions.HttpResults.Tests.Shared;
using CSharpFunctionalExtensions.HttpResults.Tests.Utils;
using FluentAssertions;
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

    var result =
      Result
        .Success<IAsyncEnumerable<int>, DocumentMissingError>(asyncEnumerable)
        .ToServerSentEventsHttpResult(eventType)
        .Result as ServerSentEventsResult<int>;

    var (response, values) = await result!.ExecuteAndGetResponseAndValues();

    result!.StatusCode.Should().Be(200);
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

    var result =
      (
        await Task.FromResult(Result.Success<IAsyncEnumerable<int>, DocumentMissingError>(asyncEnumerable))
          .ToServerSentEventsHttpResult(eventType)
      ).Result as ServerSentEventsResult<int>;

    var (response, values) = await result!.ExecuteAndGetResponseAndValues();

    result!.StatusCode.Should().Be(200);
    response.ContentType.Should().Be(MediaTypeNames.Text.EventStream);
    values.Select(v => v.Data).Should().Contain(testValues.Select(v => v.ToString()));
    values.Select(v => v.Event).Should().AllBe(eventType);
  }

  [Fact]
  public void ResultIAsyncEnumerableTE_Failure_can_be_mapped_to_ServerSentEventsHttpResult()
  {
    var error = new DocumentMissingError { DocumentId = Guid.NewGuid().ToString() };

    var result =
      Result.Failure<IAsyncEnumerable<int>, DocumentMissingError>(error).ToServerSentEventsHttpResult().Result
      as NotFound<string>;

    result!.StatusCode.Should().Be(404);
    result!.Value.Should().Be(error.DocumentId);
  }

  [Fact]
  public async Task ResultIAsyncEnumerableTE_Failure_can_be_mapped_to_ServerSentEventsHttpResult_Async()
  {
    var error = new DocumentMissingError { DocumentId = Guid.NewGuid().ToString() };

    var result =
      (
        await Task.FromResult(Result.Failure<IAsyncEnumerable<int>, DocumentMissingError>(error))
          .ToServerSentEventsHttpResult()
      ).Result as NotFound<string>;

    result!.StatusCode.Should().Be(404);
    result!.Value.Should().Be(error.DocumentId);
  }
}

#endif
