using CSharpFunctionalExtensions.HttpResults.Generators.Models;

namespace CSharpFunctionalExtensions.HttpResults.Generators.Rendering;

/// <summary>
///   Renders the built-in mapping methods for string-error results. The static variants replicate the
///   former handwritten behavior; the context variants resolve an
///   <see cref="CSharpFunctionalExtensions.HttpResults.IResultProblemDetailsProvider" /> from
///   <c>HttpContext.RequestServices</c> on failure.
/// </summary>
internal static class StringErrorMethodRenderer
{
  private const string StandardTailParameters =
    "int failureStatusCode = 400, Action<ProblemDetails>? customizeProblemDetails = null";

  private const string ProviderGuard =
    "A HttpContext is required to resolve the registered IResultProblemDetailsProvider.";

  private const string StaticFailureBlock = """
        var problemDetailsInfo = ProblemDetailsMappingProvider.FindMapping(failureStatusCode);
        var problemDetails = new ProblemDetails
        {
            Status = failureStatusCode,
            Title = problemDetailsInfo.Title,
            Type = problemDetailsInfo.Type,
            Detail = result.Error,
        };

        customizeProblemDetails?.Invoke(problemDetails);

        return TypedResults.Problem(problemDetails);
    """;

  private const string ContextFailureBlock = """
        if (httpContext is null)
            throw new InvalidOperationException(
                "A HttpContext is required to resolve the registered IResultProblemDetailsProvider.");

        var problemDetails = httpContext.RequestServices.GetRequiredService<IResultProblemDetailsProvider>()
            .CreateProblemDetails(httpContext, result.Error, failureStatusCode);

        customizeProblemDetails?.Invoke(problemDetails);

        return TypedResults.Problem(problemDetails);
    """;

  public static string GenerateStatic(HttpResultMethodDefinition method)
  {
    var summary = method.BuildSummary(customError: false);
    return SyncAsyncMethodPairRenderer.Render(
      method,
      summary,
      BuildSignature(method, includeContext: false, isAsync: false),
      BuildSignature(method, includeContext: false, isAsync: true),
      StaticFailureBlock,
      BuildForwardCall(method, includeContext: false, awaitReceiver: true)
    );
  }

  public static string GenerateWithContext(HttpResultMethodDefinition method)
  {
    var summary =
      $"{method.BuildSummary(customError: false)} Resolves the registered <see cref=\"IResultProblemDetailsProvider\"/> from request services for failures.";
    return SyncAsyncMethodPairRenderer.Render(
      method,
      summary,
      BuildSignature(method, includeContext: true, isAsync: false),
      BuildSignature(method, includeContext: true, isAsync: true),
      ContextFailureBlock,
      BuildForwardCall(method, includeContext: true, awaitReceiver: true)
    );
  }

  private static string BuildParameters(HttpResultMethodDefinition method, bool includeContext, bool isAsync)
  {
    var parameters = new List<string> { $"this {method.GetStringReceiver(isAsync)} result" };
    var httpContextAdded = false;

    foreach (var parameter in method.Parameters)
    {
      if (includeContext && !httpContextAdded && !parameter.IsRequired)
      {
        parameters.Add("HttpContext httpContext");
        httpContextAdded = true;
      }

      parameters.Add(parameter.ToString());
    }

    if (includeContext && !httpContextAdded)
      parameters.Add("HttpContext httpContext");

    parameters.Add(StandardTailParameters);

    return string.Join(", ", parameters);
  }

  private static string BuildSignature(HttpResultMethodDefinition method, bool includeContext, bool isAsync)
  {
    var generic = method.HasTypeParameter ? "<T>" : "";
    var asyncKeyword = isAsync ? "async " : "";
    var returnType = isAsync
      ? $"Task<Results<{method.SuccessArm}, ProblemHttpResult>>"
      : $"Results<{method.SuccessArm}, ProblemHttpResult>";

    return $"public static {asyncKeyword}{returnType} {method.MethodName}{generic}({BuildParameters(method, includeContext, isAsync)}){method.TypeConstraints}";
  }

  private static string BuildForwardCall(HttpResultMethodDefinition method, bool includeContext, bool awaitReceiver)
  {
    var arguments = new List<string> { awaitReceiver ? "await result" : "result" };

    foreach (var required in method.Parameters.Where(p => p.IsRequired))
      arguments.Add(required.Name);

    if (includeContext)
      arguments.Add("httpContext");

    foreach (var optional in method.Parameters.Where(p => !p.IsRequired))
      arguments.Add(optional.Name);

    arguments.Add("failureStatusCode, customizeProblemDetails");

    return $"{method.MethodName}{(method.HasTypeParameter ? "<T>" : "")}({string.Join(", ", arguments)})";
  }
}
