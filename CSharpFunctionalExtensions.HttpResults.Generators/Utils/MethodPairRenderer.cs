using System.Text;

namespace CSharpFunctionalExtensions.HttpResults.Generators.Utils;

/// <summary>Renders the common sync/async pair and optional target-framework guard for a method family.</summary>
internal static class MethodPairRenderer
{
  public static string Render(
    HttpResultMethodDefinition method,
    string summary,
    string syncSignature,
    string asyncSignature,
    string failureBlock,
    string asyncForwardCall
  )
  {
    var builder = new StringBuilder();

    if (!string.IsNullOrEmpty(method.PreprocessorDirective))
      builder.AppendLine($"#if {method.PreprocessorDirective}");

    AppendSummary(builder, summary);
    builder.AppendLine(syncSignature + " {");
    builder.AppendLine($"    if (result.IsSuccess) return {method.SuccessExpression};");
    builder.AppendLine();
    builder.AppendLine(failureBlock);
    builder.AppendLine("}");
    builder.AppendLine();
    AppendSummary(builder, summary);
    builder.AppendLine(asyncSignature + " {");
    builder.AppendLine($"    return {asyncForwardCall};");
    builder.AppendLine("}");

    if (!string.IsNullOrEmpty(method.PreprocessorDirective))
      builder.AppendLine("#endif");

    return builder.ToString();
  }

  private static void AppendSummary(StringBuilder builder, string summary)
  {
    builder.AppendLine("/// <summary>");
    builder.AppendLine($"/// {summary}");
    builder.AppendLine("/// </summary>");
  }
}
