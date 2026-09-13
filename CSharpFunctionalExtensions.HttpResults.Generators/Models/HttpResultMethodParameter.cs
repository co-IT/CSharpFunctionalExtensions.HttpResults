namespace CSharpFunctionalExtensions.HttpResults.Generators.Models;

/// <summary>Strongly typed metadata for one parameter in a generated mapping method.</summary>
internal readonly record struct HttpResultMethodParameter(MethodParameterKind Kind)
{
  public string Type =>
    Kind switch
    {
      MethodParameterKind.JsonSuccessStatusCode or MethodParameterKind.EmptySuccessStatusCode => "int",
      MethodParameterKind.OptionalUri => "Func<T, Uri>?",
      MethodParameterKind.RequiredUri => "Func<T, Uri>",
      MethodParameterKind.RouteName
      or MethodParameterKind.ContentType
      or MethodParameterKind.FileDownloadName
      or MethodParameterKind.EventType => "string?",
      MethodParameterKind.RouteValues => "Func<T, object>?",
      MethodParameterKind.LastModified => "DateTimeOffset?",
      MethodParameterKind.EntityTag => "EntityTagHeaderValue?",
      MethodParameterKind.EnableRangeProcessing => "bool",
      MethodParameterKind.ContentEncoding => "Encoding?",
      MethodParameterKind.ContentStatusCode => "int?",
      _ => throw new ArgumentOutOfRangeException(nameof(Kind), Kind, null),
    };

  public string Name =>
    Kind switch
    {
      MethodParameterKind.JsonSuccessStatusCode or MethodParameterKind.EmptySuccessStatusCode => "successStatusCode",
      MethodParameterKind.OptionalUri or MethodParameterKind.RequiredUri => "uri",
      MethodParameterKind.RouteName => "routeName",
      MethodParameterKind.RouteValues => "routeValues",
      MethodParameterKind.ContentType => "contentType",
      MethodParameterKind.FileDownloadName => "fileDownloadName",
      MethodParameterKind.LastModified => "lastModified",
      MethodParameterKind.EntityTag => "entityTag",
      MethodParameterKind.EnableRangeProcessing => "enableRangeProcessing",
      MethodParameterKind.ContentEncoding => "contentEncoding",
      MethodParameterKind.ContentStatusCode => "statusCode",
      MethodParameterKind.EventType => "eventType",
      _ => throw new ArgumentOutOfRangeException(nameof(Kind), Kind, null),
    };

  public string? DefaultValue =>
    Kind switch
    {
      MethodParameterKind.RequiredUri => null,
      MethodParameterKind.JsonSuccessStatusCode => "200",
      MethodParameterKind.EmptySuccessStatusCode => "204",
      MethodParameterKind.EnableRangeProcessing => "false",
      _ => "null",
    };

  public bool IsRequired => DefaultValue is null;

  public override string ToString() => IsRequired ? $"{Type} {Name}" : $"{Type} {Name} = {DefaultValue}";
}
