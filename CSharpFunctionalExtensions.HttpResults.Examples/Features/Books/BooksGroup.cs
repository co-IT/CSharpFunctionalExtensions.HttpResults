using CSharpFunctionalExtensions.HttpResults.Examples.Features.Books.AddBook;
using CSharpFunctionalExtensions.HttpResults.Examples.Features.Books.DeleteBook;
using CSharpFunctionalExtensions.HttpResults.Examples.Features.Books.FindBook;
using CSharpFunctionalExtensions.HttpResults.Examples.Features.Books.FindBookCover;
using CSharpFunctionalExtensions.HttpResults.Examples.Features.Books.GetBooks;
using CSharpFunctionalExtensions.HttpResults.Examples.Features.Books.UpdateBook;

namespace CSharpFunctionalExtensions.HttpResults.Examples.Features.Books;

public static class BooksGroup
{
  public static IEndpointRouteBuilder MapBooksGroup(this IEndpointRouteBuilder endpointRouteBuilder)
  {
    endpointRouteBuilder
      .MapGroup("/books")
      .WithOpenApi()
      .MapGetBooks()
      .MapFindBook()
      .MapFindBookCover()
      .MapAddBook()
      .MapUpdateBook()
      .MapDeleteBook();

    return endpointRouteBuilder;
  }
}
