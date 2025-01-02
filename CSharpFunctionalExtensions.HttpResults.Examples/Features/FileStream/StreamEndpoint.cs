using System.Net.Mime;
using CSharpFunctionalExtensions.HttpResults.ResultExtensions;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CSharpFunctionalExtensions.HttpResults.Examples.Features.FileStream;

public static class StreamEndpoint
{
  public static IEndpointRouteBuilder MapStream(this IEndpointRouteBuilder endpointRouteBuilder)
  {
    endpointRouteBuilder.MapGet("/stream", Handle).WithName(nameof(StreamEndpoint));

    return endpointRouteBuilder;
  }

  private static Results<FileStreamHttpResult, ProblemHttpResult> Handle()
  {
    return Result
      .Of(File.OpenRead("./Features/FileStream/Foo.txt"))
      .ToFileStreamHttpResult(MediaTypeNames.Text.Plain, "Foo.txt");
  }
}
