using CSharpFunctionalExtensions.HttpResults.ResultExtensions;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CSharpFunctionalExtensions.HttpResults.Examples.Features.Books.DeleteBook;

public static class DeleteBookEndpoint
{
  public static IEndpointRouteBuilder MapDeleteBook(this IEndpointRouteBuilder endpointRouteBuilder)
  {
    endpointRouteBuilder.MapDelete("{id:guid}", Handle).WithName(nameof(DeleteBookEndpoint));

    return endpointRouteBuilder;
  }

  private static Results<NoContent, ProblemHttpResult> Handle(BookService service, Guid id)
  {
    return service.Delete(id).ToNoContentHttpResult();
  }
}
