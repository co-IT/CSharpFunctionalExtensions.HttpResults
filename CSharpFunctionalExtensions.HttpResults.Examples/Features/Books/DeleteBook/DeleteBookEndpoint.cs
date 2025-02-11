using CSharpFunctionalExtensions.HttpResults.ResultExtensions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace CSharpFunctionalExtensions.HttpResults.Examples.Features.Books.DeleteBook;

public static class DeleteBookEndpoint
{
  public static IEndpointRouteBuilder MapDeleteBook(this IEndpointRouteBuilder endpointRouteBuilder)
  {
    endpointRouteBuilder.MapDelete("{id:guid}", Handle).WithName(nameof(DeleteBookEndpoint));

    return endpointRouteBuilder;
  }

  private static Results<NoContent, ProblemHttpResult> Handle([FromRoute] Guid id, [FromServices] BookService service)
  {
    return service.Delete(id).ToNoContentHttpResult();
  }
}
