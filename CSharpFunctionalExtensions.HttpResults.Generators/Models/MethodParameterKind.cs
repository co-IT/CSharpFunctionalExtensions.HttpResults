namespace CSharpFunctionalExtensions.HttpResults.Generators.Models;

/// <summary>A supported parameter in a generated mapping-method signature.</summary>
internal enum MethodParameterKind
{
  JsonSuccessStatusCode,
  EmptySuccessStatusCode,
  OptionalUri,
  RequiredUri,
  RouteName,
  RouteValues,
  ContentType,
  FileDownloadName,
  LastModified,
  EntityTag,
  EnableRangeProcessing,
  ContentEncoding,
  ContentStatusCode,
  EventType,
}
