using AwesomeAssertions;
using CSharpFunctionalExtensions.HttpResults.Tests.Shared;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CSharpFunctionalExtensions.HttpResults.Tests.ResultExtensions;

public class LegacyProblemMapperCallbackTests
{
  [Fact]
  public void Callback_modifies_the_exact_ProblemHttpResult_returned_by_a_legacy_mapper()
  {
    var mapped =
      Result
        .Failure<string, LegacyProblemError>(new LegacyProblemError("failure"))
        .ToOkHttpResult(customizeProblemDetails: details => details.Title = "custom")
        .Result as ProblemHttpResult;

    mapped!.ProblemDetails.Detail.Should().Be("failure");
    mapped.ProblemDetails.Title.Should().Be("custom");
  }

  [Fact]
  public async Task Async_callback_modifies_the_exact_ProblemHttpResult_returned_by_a_legacy_mapper()
  {
    var mapped =
      (
        await Task.FromResult(Result.Failure<string, LegacyProblemError>(new LegacyProblemError("failure")))
          .ToOkHttpResult(customizeProblemDetails: details => details.Title = "custom")
      ).Result as ProblemHttpResult;

    mapped!.ProblemDetails.Detail.Should().Be("failure");
    mapped.ProblemDetails.Title.Should().Be("custom");
  }
}
