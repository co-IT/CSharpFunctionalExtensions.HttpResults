using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace CSharpFunctionalExtensions.HttpResults.Tests.Shared;

/// <summary>
///   A test <see cref="HttpContext" /> whose request services are owned by the instance; disposing
///   releases the service provider. Converts implicitly to <see cref="HttpContext" />, so instances
///   can be passed directly to the mapping methods.
/// </summary>
internal sealed record TestHttpContext(HttpContext context, ServiceProvider serviceProvider) : IDisposable
{
  internal HttpContext Context => context;

  internal RecordingResultProblemDetailsProvider? Recorder { get; init; }

  public void Dispose() => serviceProvider.Dispose();

  public static implicit operator HttpContext(TestHttpContext request) => request.Context;
}
