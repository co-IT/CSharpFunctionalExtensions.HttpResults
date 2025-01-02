using CSharpFunctionalExtensions.HttpResults.ResultExtensions;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CSharpFunctionalExtensions.HttpResults.Examples.Features.Books.FindBookCover;

public static class FindBookCoverEndpoint
{
  public static IEndpointRouteBuilder MapFindBookCover(this IEndpointRouteBuilder endpointRouteBuilder)
  {
    endpointRouteBuilder.MapGet("{id:guid}/cover", Handle).WithName(nameof(FindBookCoverEndpoint));

    return endpointRouteBuilder;
  }

  private static Results<FileContentHttpResult, ProblemHttpResult> Handle(BookService service, Guid id)
  {
    return Maybe
      .From(service.Find(id))
      .ToResult($"Couldn't find book with id {id}.")
      .Map(book => book.Cover)
      .EnsureNotNull("No cover available.")
      .ToFileHttpResult();
  }
}
