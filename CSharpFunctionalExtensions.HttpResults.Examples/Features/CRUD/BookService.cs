namespace CSharpFunctionalExtensions.HttpResults.Examples.Features.CRUD;

public class BookService
{
  private readonly List<Book> _books = FakeBooks.Generate(20);

  public List<Book> Get()
  {
    return _books;
  }

  public Book? Find(Guid id)
  {
    return _books.FirstOrDefault(b => b.Id == id);
  }

  public Result<Book> Add(Book book)
  {
    if (_books.Any(b => b.Id == book.Id))
      return Result.Failure<Book>($"Book with id {book.Id} already exists.");

    _books.Add(book);

    return Result.Success(book);
  }

  public Result<Book> Update(Book book)
  {
    if (_books.All(b => b.Id != book.Id))
      return Result.Failure<Book>($"Book with id {book.Id} does not exist.");

    var index = _books.FindIndex(b => b.Id == book.Id);

    _books[index] = book;

    return Result.Success(book);
  }

  public Result Delete(Guid id)
  {
    if (_books.All(b => b.Id != id))
      return Result.Failure($"Book with id {id} does not exist.");

    _books.RemoveAll(book => book.Id == id);

    return Result.Success();
  }
}
