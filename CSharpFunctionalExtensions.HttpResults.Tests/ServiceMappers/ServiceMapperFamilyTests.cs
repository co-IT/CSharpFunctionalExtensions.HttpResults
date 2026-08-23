using AwesomeAssertions;
using CSharpFunctionalExtensions;
using CSharpFunctionalExtensions.HttpResults.ResultExtensions;
using CSharpFunctionalExtensions.HttpResults.Tests.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.DependencyInjection;

namespace CSharpFunctionalExtensions.HttpResults.Tests.ServiceMappers;

/// <summary>
///   Exercises the generated overloads for IServiceResultErrorMapper mappers across every
///   mapping family. ServiceDocumentErrorMapper returns ProblemHttpResult, which is a valid
///   failure arm for all built-in unions.
/// </summary>
public class ServiceMapperFamilyTests
{
  private static TestHttpContext Context()
  {
    var services = new ServiceCollection();
    services.AddSingleton<ServiceMapperDependency>();
    services.AddScoped<ServiceDocumentErrorMapper>();
    return TestHttpContextHelper.Create(services);
  }

  private static ServiceDocumentError Error => new() { DocumentId = "d1" };

  [Fact]
  public void Ok_failure_is_resolved_from_request_services()
  {
    using var context = Context();

    var result =
      Result.Failure<string, ServiceDocumentError>(Error).ToOkHttpResult(context).Result as ProblemHttpResult;

    result!.ProblemDetails.Detail.Should().Be("resolved-by-di:d1");
  }

  [Fact]
  public void StatusCode_failure_is_resolved_from_request_services()
  {
    using var context = Context();

    var result =
      Result.Failure<string, ServiceDocumentError>(Error).ToStatusCodeHttpResult(context, successStatusCode: 200).Result
      as ProblemHttpResult;

    result!.StatusCode.Should().Be(404);
  }

  [Fact]
  public void Json_failure_is_resolved_from_request_services()
  {
    using var context = Context();

    var result =
      Result.Failure<string, ServiceDocumentError>(Error).ToJsonHttpResult(context).Result as ProblemHttpResult;

    result!.ProblemDetails.Title.Should().Be("Document not found");
  }

  [Fact]
  public void NoContent_failure_for_ResultTE_is_resolved_from_request_services()
  {
    using var context = Context();

    var result =
      Result.Failure<string, ServiceDocumentError>(Error).ToNoContentHttpResult(context).Result as ProblemHttpResult;

    result!.ProblemDetails.Detail.Should().Be("resolved-by-di:d1");
  }

  [Fact]
  public void NoContent_failure_for_UnitResult_is_resolved_from_request_services()
  {
    using var context = Context();

    var result =
      UnitResult.Failure<ServiceDocumentError>(Error).ToNoContentHttpResult(context).Result as ProblemHttpResult;

    result!.ProblemDetails.Detail.Should().Be("resolved-by-di:d1");
  }

  [Fact]
  public void StatusCode_failure_for_UnitResult_is_resolved_from_request_services()
  {
    using var context = Context();

    var result =
      UnitResult.Failure<ServiceDocumentError>(Error).ToStatusCodeHttpResult(context).Result as ProblemHttpResult;

    result!.ProblemDetails.Detail.Should().Be("resolved-by-di:d1");
  }

  [Fact]
  public void Created_failure_is_resolved_from_request_services()
  {
    using var context = Context();

    var result =
      Result
        .Failure<string, ServiceDocumentError>(Error)
        .ToCreatedHttpResult(context, uri: _ => new Uri("https://example.test/created"))
        .Result as ProblemHttpResult;

    result!.ProblemDetails.Detail.Should().Be("resolved-by-di:d1");
  }

  [Fact]
  public async Task Async_Created_overload_forwards_the_context()
  {
    using var context = Context();

    var result =
      (await Task.FromResult(Result.Failure<string, ServiceDocumentError>(Error)).ToCreatedHttpResult(context)).Result
      as ProblemHttpResult;

    result!.ProblemDetails.Detail.Should().Be("resolved-by-di:d1");
  }

  [Fact]
  public void CreatedAtRoute_failure_is_resolved_from_request_services()
  {
    using var context = Context();

    var result =
      Result
        .Failure<string, ServiceDocumentError>(Error)
        .ToCreatedAtRouteHttpResult(context, routeValues: _ => new { id = "d1" })
        .Result as ProblemHttpResult;

    result!.ProblemDetails.Detail.Should().Be("resolved-by-di:d1");
  }

  [Fact]
  public void Accepted_failure_with_required_uri_is_resolved_from_request_services()
  {
    using var context = Context();

    var result =
      Result
        .Failure<string, ServiceDocumentError>(Error)
        .ToAcceptedHttpResult(_ => new Uri("https://example.test/accepted"), context)
        .Result as ProblemHttpResult;

    result!.ProblemDetails.Detail.Should().Be("resolved-by-di:d1");
  }

  [Fact]
  public void AcceptedAtRoute_failure_is_resolved_from_request_services()
  {
    using var context = Context();

    var result =
      Result
        .Failure<string, ServiceDocumentError>(Error)
        .ToAcceptedAtRouteHttpResult(context, routeName: "document")
        .Result as ProblemHttpResult;

    result!.ProblemDetails.Detail.Should().Be("resolved-by-di:d1");
  }

  [Fact]
  public void FileContent_failure_is_resolved_from_request_services()
  {
    using var context = Context();

    var result =
      Result.Failure<byte[], ServiceDocumentError>(Error).ToFileHttpResult(context).Result as ProblemHttpResult;

    result!.ProblemDetails.Detail.Should().Be("resolved-by-di:d1");
  }

  [Fact]
  public void FileStream_failure_is_resolved_from_request_services()
  {
    using var context = Context();

    var result =
      Result.Failure<Stream, ServiceDocumentError>(Error).ToFileStreamHttpResult(context).Result as ProblemHttpResult;

    result!.ProblemDetails.Detail.Should().Be("resolved-by-di:d1");
  }

  [Fact]
  public void Content_failure_is_resolved_from_request_services()
  {
    using var context = Context();

    var result =
      Result.Failure<string, ServiceDocumentError>(Error).ToContentHttpResult(context).Result as ProblemHttpResult;

    result!.ProblemDetails.Detail.Should().Be("resolved-by-di:d1");
  }

#if NET10_0_OR_GREATER
  [Fact]
  public async Task ServerSentEvents_failure_is_resolved_from_request_services()
  {
    using var context = Context();

    var result =
      (
        await Task.FromResult(Result.Failure<IAsyncEnumerable<string>, ServiceDocumentError>(Error))
          .ToServerSentEventsHttpResult(context)
      ).Result as ProblemHttpResult;

    result!.ProblemDetails.Detail.Should().Be("resolved-by-di:d1");
  }
#endif

  [Fact]
  public void Missing_context_fails_fast_with_a_descriptive_exception()
  {
    var act = () => Result.Failure<string, ServiceDocumentError>(Error).ToOkHttpResult((HttpContext)null!);

    act.Should()
      .Throw<InvalidOperationException>()
      .WithMessage("A HttpContext is required for IServiceResultErrorMapper mappings.");
  }

  [Fact]
  public void Success_paths_never_resolve_the_mapper()
  {
    var services = new ServiceCollection();
    services.AddScoped<ThrowingCtorMapper>();
    using var context = TestHttpContextHelper.Create(services);

    // The success arm of the very error the throwing mapper is registered for:
    // resolving it would throw in its constructor.
    var act = () => Result.Success<string, ThrowingCtorError>("ok").ToOkHttpResult(context);

    act.Should().NotThrow();
  }

  [Fact]
  public void Mapper_returning_ProblemHttpResult_supports_customize_callback()
  {
    using var context = Context();

    var result =
      Result
        .Failure<string, ServiceDocumentError>(Error)
        .ToOkHttpResult(context, customizeProblemDetails: p => p.Title = "Edge Case")
        .Result as ProblemHttpResult;

    result!.ProblemDetails.Title.Should().Be("Edge Case");
    result.ProblemDetails.Detail.Should().Be("resolved-by-di:d1");
  }
}
