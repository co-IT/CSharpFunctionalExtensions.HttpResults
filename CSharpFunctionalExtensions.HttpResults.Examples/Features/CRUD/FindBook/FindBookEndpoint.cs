using System.Net;
using CSharpFunctionalExtensions.HttpResults.ResultExtensions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace CSharpFunctionalExtensions.HttpResults.Examples.Features.CRUD.FindBook;

public static class FindBookEndpoint
{
  public static IEndpointRouteBuilder MapFindBook(this IEndpointRouteBuilder endpointRouteBuilder)
  {
    endpointRouteBuilder.MapGet("{id:guid}", Handle).WithName(nameof(FindBookEndpoint));

    return endpointRouteBuilder;
  }

  private static Results<Ok<Book>, ProblemHttpResult> Handle([FromRoute] Guid id, [FromServices] BookService service)
  {
    return Maybe
      .From(service.Find(id))
      .ToResult($"Couldn't find book with id {id}.")
      .ToOkHttpResult((int)HttpStatusCode.NotFound);
  }
}
