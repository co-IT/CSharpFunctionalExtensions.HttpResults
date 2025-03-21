namespace CSharpFunctionalExtensions.HttpResults.Examples.Features.CRUD;

public class Book
{
  private Book(string title, string author, byte[]? cover)
  {
    Id = Guid.NewGuid();
    Title = title;
    Author = author;
    Cover = cover;
  }

  public Guid Id { get; }
  public string Title { get; private set; }
  public string Author { get; private set; }
  public byte[]? Cover { get; private set; }

  public static Result<Book> Create(string title, string author, byte[]? cover = null)
  {
    if (string.IsNullOrWhiteSpace(title))
      return Result.Failure<Book>("Title is required.");

    if (string.IsNullOrWhiteSpace(author))
      return Result.Failure<Book>("Author is required.");

    return new Book(title, author, cover);
  }

  public Result Update(string title, string author, byte[]? cover)
  {
    if (string.IsNullOrWhiteSpace(title))
      return Result.Failure<Book>("Title is required.");

    Title = title;

    if (string.IsNullOrWhiteSpace(author))
      return Result.Failure<Book>("Author is required.");

    Author = author;

    Cover = cover;

    return Result.Success();
  }
}
