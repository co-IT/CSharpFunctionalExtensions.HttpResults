#if NET10_0_OR_GREATER

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.DependencyInjection;

namespace CSharpFunctionalExtensions.HttpResults.Tests.Utils;

public static class ServerSentEventsExtensions
{
  public static async Task<(
    HttpResponse Response,
    IReadOnlyList<EventMessage> Values
  )> ExecuteAndGetResponseAndValues<T>(this ServerSentEventsResult<T> result)
  {
    var httpContext = new DefaultHttpContext
    {
      Response = { Body = new MemoryStream() },
      RequestServices = new ServiceCollection().BuildServiceProvider(),
    };
    await result.ExecuteAsync(httpContext);
    httpContext.Response.Body.Seek(0, SeekOrigin.Begin);

    var body = await new StreamReader(httpContext.Response.Body).ReadToEndAsync();

    var values = body.Split("\n\n", StringSplitOptions.RemoveEmptyEntries)
      .Select(block =>
      {
        var lines = block.Split('\n', StringSplitOptions.RemoveEmptyEntries);

        string? eventType = null;
        var data = string.Empty;

        foreach (var line in lines)
        {
          if (line.StartsWith("event: "))
            eventType = line["event: ".Length..].Trim();
          else if (line.StartsWith("data: "))
            data = line["data: ".Length..].Trim();
        }

        return new EventMessage { Event = eventType, Data = data };
      })
      .ToList()
      .AsReadOnly();

    return (httpContext.Response, values);
  }

  public record EventMessage
  {
    public string? Event { get; init; }
    public string Data { get; init; }
  }
}

#endif
