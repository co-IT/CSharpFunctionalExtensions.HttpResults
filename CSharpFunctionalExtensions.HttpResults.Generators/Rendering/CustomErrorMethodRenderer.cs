using CSharpFunctionalExtensions.HttpResults.Generators.Models;

namespace CSharpFunctionalExtensions.HttpResults.Generators.Rendering;

/// <summary>
///   Renders the sync and async extension-method overloads for one custom-error method family
///   and one discovered error mapper. Standard mappers resolve through their static instance,
///   service mappers through <c>HttpContext.RequestServices</c>.
/// </summary>
internal static class CustomErrorMethodRenderer
{
  private const string ServiceGuard = "A HttpContext is required for IServiceResultErrorMapper mappings.";

  public static string Generate(in MapperGenerationContext context, HttpResultMethodDefinition method)
  {
    var summary = BuildSummary(context, method);
    return SyncAsyncMethodPairRenderer.Render(
      method,
      summary,
      BuildSignature(context, method, isAsync: false),
      BuildSignature(context, method, isAsync: true),
      FailureBlock(context),
      BuildForwardCall(context, method, awaitReceiver: true)
    );
  }

  private static string BuildSummary(in MapperGenerationContext context, HttpResultMethodDefinition method)
  {
    var summary = method.BuildSummary(customError: true);

    if (context.IsServiceMapper)
      summary +=
        " Requires an <see cref=\"Microsoft.AspNetCore.Http.HttpContext\"/> argument to resolve the registered <see cref=\"CSharpFunctionalExtensions.HttpResults.IServiceResultErrorMapper{TError, THttpResult}\"/> from request services on failure.";

    if (context.IsProblemHttpResult)
      summary += " The callback is applied to the returned ProblemHttpResult.";

    return summary;
  }

  private static string BuildParameters(
    in MapperGenerationContext context,
    HttpResultMethodDefinition method,
    bool isAsync
  )
  {
    var parameters = new List<string> { $"this {method.GetCustomReceiver(context.ErrorType, isAsync)} result" };

    var httpContextAdded = false;

    foreach (var parameter in method.Parameters)
    {
      if (context.IsServiceMapper && !httpContextAdded && !parameter.IsRequired)
      {
        parameters.Add("HttpContext httpContext");
        httpContextAdded = true;
      }

      parameters.Add(parameter.ToString());
    }

    if (context.IsServiceMapper && !httpContextAdded)
      parameters.Add("HttpContext httpContext");

    if (context.IsProblemHttpResult)
      parameters.Add("Action<ProblemDetails>? customizeProblemDetails = null");

    return string.Join(", ", parameters);
  }

  private static string BuildSignature(
    in MapperGenerationContext context,
    HttpResultMethodDefinition method,
    bool isAsync
  )
  {
    var generic = method.HasTypeParameter ? "<T>" : "";
    var asyncKeyword = isAsync ? "async " : "";
    var returnType = isAsync
      ? $"Task<Results<{method.SuccessArm}, {context.HttpResultType}>>"
      : $"Results<{method.SuccessArm}, {context.HttpResultType}>";

    return $"public static {asyncKeyword}{returnType} {method.MethodName}{generic}({BuildParameters(context, method, isAsync)}){method.TypeConstraints}";
  }

  private static string BuildForwardArguments(
    in MapperGenerationContext context,
    HttpResultMethodDefinition method,
    bool awaitReceiver
  )
  {
    var arguments = new List<string> { awaitReceiver ? "await result" : "result" };

    foreach (var parameter in method.Parameters.Where(parameter => parameter.IsRequired))
      arguments.Add(parameter.Name);

    if (context.IsServiceMapper)
      arguments.Add("httpContext");

    foreach (var parameter in method.Parameters.Where(parameter => !parameter.IsRequired))
      arguments.Add(parameter.Name);

    if (context.IsProblemHttpResult)
      arguments.Add("customizeProblemDetails");

    return string.Join(", ", arguments);
  }

  private static string BuildForwardCall(
    in MapperGenerationContext context,
    HttpResultMethodDefinition method,
    bool awaitReceiver
  ) =>
    $"{method.MethodName}{(method.HasTypeParameter ? "<T>" : "")}({BuildForwardArguments(context, method, awaitReceiver)})";

  private static string FailureBlock(in MapperGenerationContext context)
  {
    var lines = new List<string>();

    if (context.IsServiceMapper)
    {
      lines.Add("    if (httpContext is null)");
      lines.Add($"        throw new InvalidOperationException(\"{ServiceGuard}\");");
      lines.Add("");
    }

    var mapped = context.IsServiceMapper
      ? $"(({context.MapperInterfaceType})httpContext.RequestServices.GetRequiredService<{context.MapperFullyQualifiedName}>()).Map(result.Error)"
      : $"CSharpFunctionalExtensionsHttpResultsResultErrorMapperCache.{context.CacheMemberName}.Map(result.Error)";

    if (context.IsProblemHttpResult)
    {
      lines.Add($"    var mapped = {mapped};");
      lines.Add("    customizeProblemDetails?.Invoke(mapped.ProblemDetails);");
      lines.Add("");
      lines.Add("    return mapped;");
    }
    else
    {
      lines.Add($"    return {mapped};");
    }

    return string.Join("\n", lines);
  }
}
