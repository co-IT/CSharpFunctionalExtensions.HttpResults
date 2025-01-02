namespace CSharpFunctionalExtensions.HttpResults.Examples.Features.Books;

public record AddBookRequest(string Title, string Author, byte[]? Cover);