using CSharpFunctionalExtensions.HttpResults.ResultExtensions;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CSharpFunctionalExtensions.HttpResults.Examples.Features.Books.FindBook;

public static class FindBookEndpoint
{
  public static IEndpointRouteBuilder MapFindBook(this IEndpointRouteBuilder endpointRouteBuilder)
  {
    endpointRouteBuilder.MapGet("{id:guid}", Handle).WithName(nameof(FindBookEndpoint));

    return endpointRouteBuilder;
  }

  private static Results<Ok<Book>, ProblemHttpResult> Handle(BookService service, Guid id) =>
    Maybe.From(service.Find(id)).ToResult($"Couldn't find book with id {id}.").ToOkHttpResult();
}
