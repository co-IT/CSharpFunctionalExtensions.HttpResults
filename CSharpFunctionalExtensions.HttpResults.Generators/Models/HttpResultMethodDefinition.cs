namespace CSharpFunctionalExtensions.HttpResults.Generators.Models;

/// <summary>
///   Structured metadata for one generated HTTP-result method family. All string-error and custom-error
///   generation derives from this single model so their signatures cannot drift independently.
/// </summary>
internal sealed record HttpResultMethodDefinition(HttpResultMethodKind Kind, ResultReceiverKind Receiver)
{
  public bool SupportsStringErrors => Receiver != ResultReceiverKind.UnitResult;

  public bool SupportsCustomErrors => Receiver != ResultReceiverKind.Result;

  public string MethodName =>
    Kind switch
    {
      HttpResultMethodKind.File => "ToFileHttpResult",
      _ => $"To{Kind}HttpResult",
    };

  public string SuccessArm =>
    Kind switch
    {
      HttpResultMethodKind.StatusCode => "StatusCodeHttpResult",
      HttpResultMethodKind.Json => "JsonHttpResult<T>",
      HttpResultMethodKind.Ok => "Ok<T>",
      HttpResultMethodKind.NoContent => "NoContent",
      HttpResultMethodKind.Created => "Created<T>",
      HttpResultMethodKind.CreatedAtRoute => "CreatedAtRoute<T>",
      HttpResultMethodKind.Accepted => "Accepted<T>",
      HttpResultMethodKind.AcceptedAtRoute => "AcceptedAtRoute<T>",
      HttpResultMethodKind.File => "FileContentHttpResult",
      HttpResultMethodKind.FileStream => "FileStreamHttpResult",
      HttpResultMethodKind.Content => "ContentHttpResult",
      HttpResultMethodKind.ServerSentEvents => "ServerSentEventsResult<T>",
      _ => throw new ArgumentOutOfRangeException(nameof(Kind), Kind, null),
    };

  public string SuccessExpression =>
    Kind switch
    {
      HttpResultMethodKind.StatusCode => "TypedResults.StatusCode(successStatusCode)",
      HttpResultMethodKind.Json => "TypedResults.Json(result.Value, statusCode: successStatusCode)",
      HttpResultMethodKind.Ok => "TypedResults.Ok(result.Value)",
      HttpResultMethodKind.NoContent => "TypedResults.NoContent()",
      HttpResultMethodKind.Created =>
        "uri is null ? TypedResults.Created(string.Empty, result.Value) : TypedResults.Created(uri.Invoke(result.Value), result.Value)",
      HttpResultMethodKind.CreatedAtRoute =>
        "TypedResults.CreatedAtRoute(result.Value, routeName, routeValues?.Invoke(result.Value))",
      HttpResultMethodKind.Accepted => "TypedResults.Accepted(uri(result.Value), result.Value)",
      HttpResultMethodKind.AcceptedAtRoute =>
        "TypedResults.AcceptedAtRoute(result.Value, routeName, routeValues?.Invoke(result.Value))",
      HttpResultMethodKind.File =>
        "TypedResults.File(result.Value, contentType, fileDownloadName, enableRangeProcessing, lastModified, entityTag)",
      HttpResultMethodKind.FileStream =>
        "TypedResults.Stream(result.Value, contentType, fileDownloadName, lastModified, entityTag, enableRangeProcessing)",
      HttpResultMethodKind.Content => "TypedResults.Content(result.Value, contentType, contentEncoding, statusCode)",
      HttpResultMethodKind.ServerSentEvents => "TypedResults.ServerSentEvents(result.Value, eventType)",
      _ => throw new ArgumentOutOfRangeException(nameof(Kind), Kind, null),
    };

  public IReadOnlyList<HttpResultMethodParameter> Parameters =>
    Kind switch
    {
      HttpResultMethodKind.StatusCode => [new(MethodParameterKind.EmptySuccessStatusCode)],
      HttpResultMethodKind.Json => [new(MethodParameterKind.JsonSuccessStatusCode)],
      HttpResultMethodKind.Created => [new(MethodParameterKind.OptionalUri)],
      HttpResultMethodKind.CreatedAtRoute or HttpResultMethodKind.AcceptedAtRoute =>
      [
        new(MethodParameterKind.RouteName),
        new(MethodParameterKind.RouteValues),
      ],
      HttpResultMethodKind.Accepted => [new(MethodParameterKind.RequiredUri)],
      HttpResultMethodKind.File or HttpResultMethodKind.FileStream =>
      [
        new(MethodParameterKind.ContentType),
        new(MethodParameterKind.FileDownloadName),
        new(MethodParameterKind.LastModified),
        new(MethodParameterKind.EntityTag),
        new(MethodParameterKind.EnableRangeProcessing),
      ],
      HttpResultMethodKind.Content =>
      [
        new(MethodParameterKind.ContentType),
        new(MethodParameterKind.ContentEncoding),
        new(MethodParameterKind.ContentStatusCode),
      ],
      HttpResultMethodKind.ServerSentEvents => [new(MethodParameterKind.EventType)],
      _ => [],
    };

  public bool HasTypeParameter =>
    Receiver
      is ResultReceiverKind.ResultOfT
        or ResultReceiverKind.ResultOfStream
        or ResultReceiverKind.ResultOfAsyncEnumerable;

  public string TypeConstraints => Receiver == ResultReceiverKind.ResultOfStream ? " where T : Stream" : "";

  public string PreprocessorDirective => Kind == HttpResultMethodKind.ServerSentEvents ? "NET10_0_OR_GREATER" : "";

  public string GetStringReceiver(bool isAsync)
  {
    var receiver = Receiver switch
    {
      ResultReceiverKind.Result => "Result",
      ResultReceiverKind.ResultOfT or ResultReceiverKind.ResultOfStream => "Result<T>",
      ResultReceiverKind.ResultOfByteArray => "Result<byte[]>",
      ResultReceiverKind.ResultOfString => "Result<string>",
      ResultReceiverKind.ResultOfAsyncEnumerable => "Result<IAsyncEnumerable<T>>",
      _ => throw new InvalidOperationException($"{Receiver} does not support string-error mapping."),
    };

    return isAsync ? $"Task<{receiver}>" : receiver;
  }

  public string GetCustomReceiver(string errorType, bool isAsync)
  {
    var receiver = Receiver switch
    {
      ResultReceiverKind.ResultOfT or ResultReceiverKind.ResultOfStream => $"Result<T,{errorType}>",
      ResultReceiverKind.ResultOfByteArray => $"Result<byte[],{errorType}>",
      ResultReceiverKind.ResultOfString => $"Result<string,{errorType}>",
      ResultReceiverKind.ResultOfAsyncEnumerable => $"Result<IAsyncEnumerable<T>,{errorType}>",
      ResultReceiverKind.UnitResult => $"UnitResult<{errorType}>",
      _ => throw new InvalidOperationException($"{Receiver} does not support custom-error mapping."),
    };

    return isAsync ? $"Task<{receiver}>" : receiver;
  }

  public string BuildSummary(bool customError)
  {
    var valueText = Kind switch
    {
      HttpResultMethodKind.StatusCode => "a <see cref=\"StatusCodeHttpResult\"/>",
      HttpResultMethodKind.Json => "a <see cref=\"JsonHttpResult{TValue}\"/>",
      HttpResultMethodKind.Ok => "an <see cref=\"Ok{TValue}\"/>",
      HttpResultMethodKind.NoContent => "a <see cref=\"NoContent\"/>",
      HttpResultMethodKind.Created => "a <see cref=\"Created{TValue}\"/>",
      HttpResultMethodKind.CreatedAtRoute => "a <see cref=\"CreatedAtRoute{TValue}\"/>",
      HttpResultMethodKind.Accepted => "an <see cref=\"Accepted{TValue}\"/>",
      HttpResultMethodKind.AcceptedAtRoute => "an <see cref=\"AcceptedAtRoute{TValue}\"/>",
      HttpResultMethodKind.File => "a <see cref=\"FileContentHttpResult\"/>",
      HttpResultMethodKind.FileStream => "a <see cref=\"FileStreamHttpResult\"/>",
      HttpResultMethodKind.Content => "a <see cref=\"ContentHttpResult\"/>",
      HttpResultMethodKind.ServerSentEvents => "a <see cref=\"ServerSentEventsResult{T}\"/>",
      _ => throw new ArgumentOutOfRangeException(nameof(Kind), Kind, null),
    };

    var failureText = customError
      ? "Returns the custom error mapping in case of failure."
      : "Returns a <see cref=\"ProblemHttpResult\"/> in case of failure.";
    return $"Returns {valueText} in case of success. {failureText}";
  }
}
