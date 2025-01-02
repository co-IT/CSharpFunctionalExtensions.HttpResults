using Bogus;

namespace CSharpFunctionalExtensions.HttpResults.Examples.Features.Books;

public class FakeBooks
{
  public static List<Book> Generate(int count)
  {
    return new Faker<Book>()
      .UseSeed(420)
      .CustomInstantiator(faker => Book.Create(faker.Name.JobTitle(), faker.Name.FullName()).Value)
      .Generate(count);
  }
}
