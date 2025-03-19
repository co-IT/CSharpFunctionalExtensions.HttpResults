using CSharpFunctionalExtensions.HttpResults.Examples.Features.CRUD;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CSharpFunctionalExtensions.HttpResults.Examples.Features.MultipleErrorChain;

/// <summary>
/// Because C# does not natively support union types there is no optimal solution to handle different
/// types of errors in a chain of <see cref="Result{T,E}" />.
/// This is the best workaround I could find for now.
/// </summary>

// 1. Create base type for all errors and define concrete errors as child type

public record BookError;

public record BookNotFoundError(Guid BookId) : BookError;

public record BookArchivedError(Guid BookId, DateTimeOffset ArchivedAt) : BookError;

// 2. Create a mapper for the base type and map the concrete errors to the corresponding http result
public class BookErrorMapper : IResultErrorMapper<BookError, ProblemHttpResult>
{
  public ProblemHttpResult Map(BookError bookError)
  {
    return bookError switch
    {
      BookNotFoundError error => TypedResults.Problem(
        $"Book with Id {error.BookId} could not be found.",
        null,
        404,
        "Book not found"
      ),
      BookArchivedError error => TypedResults.Problem(
        $"Book with Id {error.BookId} already archived at {error.ArchivedAt:O}",
        null,
        400,
        "Book already archived"
      ),
      _ => TypedResults.Problem(),
    };
  }
}

public class MultipleErrorChain
{
  // 3. Use custom errors in chain, always specify the base type (so here the error type of every step in chain is BookError)
  public Results<Ok<Book>, ProblemHttpResult> ArchiveBookEndpoint(Guid bookId)
  {
    return Maybe
      .From(FindBook(bookId))
      .ToResult<Book, BookError>(new BookNotFoundError(bookId)) // specify base class either as type arguments
      .Check(book => ArchiveBook(book).MapError(error => error as BookError)) //or via extra mapping
      .ToOkHttpResult();
  }

  private static Book? FindBook(Guid id)
  {
    //sample implementation
    return null;
  }

  private static UnitResult<BookArchivedError> ArchiveBook(Book book)
  {
    //sample implementation
    return UnitResult.Failure(new BookArchivedError(book.Id, DateTimeOffset.Now));
  }
}
