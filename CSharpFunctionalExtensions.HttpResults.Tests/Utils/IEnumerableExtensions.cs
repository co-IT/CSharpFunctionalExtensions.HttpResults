namespace CSharpFunctionalExtensions.HttpResults.Tests.Utils;

public static class EnumerableExtensions
{
  public static async IAsyncEnumerable<T> AsAsyncEnumerable<T>(this IEnumerable<T> items)
  {
    foreach (var item in items)
      yield return item;

    await Task.CompletedTask;
  }
}
