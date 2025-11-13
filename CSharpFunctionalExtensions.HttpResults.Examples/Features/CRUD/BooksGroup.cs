using CSharpFunctionalExtensions.HttpResults.Examples.Features.CRUD.AddBook;
using CSharpFunctionalExtensions.HttpResults.Examples.Features.CRUD.DeleteBook;
using CSharpFunctionalExtensions.HttpResults.Examples.Features.CRUD.FindBook;
using CSharpFunctionalExtensions.HttpResults.Examples.Features.CRUD.FindBookCover;
using CSharpFunctionalExtensions.HttpResults.Examples.Features.CRUD.GetBooks;
using CSharpFunctionalExtensions.HttpResults.Examples.Features.CRUD.UpdateBook;

namespace CSharpFunctionalExtensions.HttpResults.Examples.Features.CRUD;

public static class BooksGroup
{
  public static IEndpointRouteBuilder MapBooksGroup(this IEndpointRouteBuilder endpointRouteBuilder)
  {
    endpointRouteBuilder
      .MapGroup("/books")
      .MapGetBooks()
      .MapFindBook()
      .MapFindBookCover()
      .MapAddBook()
      .MapUpdateBook()
      .MapDeleteBook();

    return endpointRouteBuilder;
  }
}
