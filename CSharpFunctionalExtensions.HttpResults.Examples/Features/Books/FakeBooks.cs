using System.Text;
using System.Web;
using Bogus;

namespace CSharpFunctionalExtensions.HttpResults.Examples.Features.Books;

public class FakeBooks
{
  public static List<Book> Generate(int count)
  {
    return new Faker<Book>()
      .UseSeed(420)
      .CustomInstantiator(faker =>
        Book.Create(
          faker.Name.JobTitle(),
          faker.Name.FullName(),
          Encoding.UTF8.GetBytes(HttpUtility.UrlDecode(faker.Image.DataUri(100, 100)).Split(",")[1])
        ).Value
      )
      .Generate(count);
  }
}
