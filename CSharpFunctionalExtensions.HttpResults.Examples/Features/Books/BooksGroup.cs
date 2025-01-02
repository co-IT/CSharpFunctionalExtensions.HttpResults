namespace CSharpFunctionalExtensions.HttpResults.Examples.Features.Books;

public static class BooksGroup
{
  public static IEndpointRouteBuilder MapBooksGroup(this IEndpointRouteBuilder endpointRouteBuilder)
  {
    endpointRouteBuilder.MapGroup("/books")
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