namespace CSharpFunctionalExtensions.HttpResults.Examples.Features.Books.UpdateBook;

public record UpdateBookRequest(string Title, string Author, byte[]? Cover);
