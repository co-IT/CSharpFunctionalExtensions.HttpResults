namespace CSharpFunctionalExtensions.HttpResults.Generators.Models;

/// <summary>The ASP.NET Core typed-result family produced on a successful result.</summary>
internal enum HttpResultMethodKind
{
  StatusCode,
  Json,
  Ok,
  NoContent,
  Created,
  CreatedAtRoute,
  Accepted,
  AcceptedAtRoute,
  File,
  FileStream,
  Content,
  ServerSentEvents,
}
