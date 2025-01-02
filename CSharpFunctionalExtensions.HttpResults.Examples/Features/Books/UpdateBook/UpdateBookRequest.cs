namespace CSharpFunctionalExtensions.HttpResults.Examples.Features.Books;

public record UpdateBookRequest(string Title, string Author, byte[]? Cover);