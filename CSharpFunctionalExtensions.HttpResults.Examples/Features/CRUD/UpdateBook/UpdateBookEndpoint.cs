using CSharpFunctionalExtensions.HttpResults.ResultExtensions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace CSharpFunctionalExtensions.HttpResults.Examples.Features.CRUD.UpdateBook;

public static class UpdateBookEndpoint
{
  public static IEndpointRouteBuilder MapUpdateBook(this IEndpointRouteBuilder endpointRouteBuilder)
  {
    endpointRouteBuilder.MapPut("{id:guid}", Handle).WithName(nameof(UpdateBookEndpoint));

    return endpointRouteBuilder;
  }

  private static Results<Ok<Book>, ProblemHttpResult> Handle(
    [FromRoute] Guid id,
    [FromServices] BookService service,
    [FromBody] UpdateBookRequest request
  )
  {
    return Maybe
      .From(service.Find(id))
      .ToResult($"Couldn't find book with id {id}.")
      .Check(book => book.Update(request.Title, request.Author, request.Cover))
      .Bind(book => service.Update(book))
      .ToOkHttpResult();
  }
}
