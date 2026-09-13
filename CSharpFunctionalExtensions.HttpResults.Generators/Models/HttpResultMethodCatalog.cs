namespace CSharpFunctionalExtensions.HttpResults.Generators.Models;

/// <summary>The single source of truth for every generated HTTP-result method family.</summary>
internal static class HttpResultMethodCatalog
{
  private static readonly IReadOnlyList<HttpResultMethodDefinition> Definitions =
  [
    new(HttpResultMethodKind.StatusCode, ResultReceiverKind.Result),
    new(HttpResultMethodKind.StatusCode, ResultReceiverKind.ResultOfT),
    new(HttpResultMethodKind.Json, ResultReceiverKind.ResultOfT),
    new(HttpResultMethodKind.Ok, ResultReceiverKind.ResultOfT),
    new(HttpResultMethodKind.NoContent, ResultReceiverKind.Result),
    new(HttpResultMethodKind.NoContent, ResultReceiverKind.ResultOfT),
    new(HttpResultMethodKind.Created, ResultReceiverKind.ResultOfT),
    new(HttpResultMethodKind.CreatedAtRoute, ResultReceiverKind.ResultOfT),
    new(HttpResultMethodKind.Accepted, ResultReceiverKind.ResultOfT),
    new(HttpResultMethodKind.AcceptedAtRoute, ResultReceiverKind.ResultOfT),
    new(HttpResultMethodKind.File, ResultReceiverKind.ResultOfByteArray),
    new(HttpResultMethodKind.FileStream, ResultReceiverKind.ResultOfStream),
    new(HttpResultMethodKind.Content, ResultReceiverKind.ResultOfString),
    new(HttpResultMethodKind.ServerSentEvents, ResultReceiverKind.ResultOfAsyncEnumerable),
    new(HttpResultMethodKind.NoContent, ResultReceiverKind.UnitResult),
    new(HttpResultMethodKind.StatusCode, ResultReceiverKind.UnitResult),
  ];

  public static IReadOnlyList<HttpResultMethodDefinition> StringErrorMethods { get; } =
    Definitions.Where(definition => definition.SupportsStringErrors).ToArray();

  public static IReadOnlyList<HttpResultMethodDefinition> ResultWithCustomErrorMethods { get; } =
    Definitions
      .Where(definition => definition.SupportsCustomErrors && definition.Receiver != ResultReceiverKind.UnitResult)
      .ToArray();

  public static IReadOnlyList<HttpResultMethodDefinition> UnitResultWithCustomErrorMethods { get; } =
    Definitions.Where(definition => definition.Receiver == ResultReceiverKind.UnitResult).ToArray();
}
