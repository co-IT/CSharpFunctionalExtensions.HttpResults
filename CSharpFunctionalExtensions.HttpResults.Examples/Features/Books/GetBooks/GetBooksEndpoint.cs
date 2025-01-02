using CSharpFunctionalExtensions.HttpResults.ResultExtensions;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CSharpFunctionalExtensions.HttpResults.Examples.Features.Books.GetBooks;

public static class GetBooksEndpoint
{
  public static IEndpointRouteBuilder MapGetBooks(this IEndpointRouteBuilder endpointRouteBuilder)
  {
    endpointRouteBuilder.MapGet("", Handle).WithName(nameof(GetBooksEndpoint));

    return endpointRouteBuilder;
  }

  private static Results<Ok<List<Book>>, ProblemHttpResult> Handle(BookService service) =>
    Result.Of(service.Get).ToOkHttpResult();
}
