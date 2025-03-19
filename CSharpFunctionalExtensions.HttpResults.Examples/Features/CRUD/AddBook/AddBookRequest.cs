namespace CSharpFunctionalExtensions.HttpResults.Examples.Features.CRUD.AddBook;

public record AddBookRequest(string Title, string Author, byte[]? Cover);
