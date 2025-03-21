namespace CSharpFunctionalExtensions.HttpResults.Examples.Features.CRUD.UpdateBook;

public record UpdateBookRequest(string Title, string Author, byte[]? Cover);
