namespace CSharpFunctionalExtensions.HttpResults.Examples.Features.Books.AddBook;

public record AddBookRequest(string Title, string Author, byte[]? Cover);
