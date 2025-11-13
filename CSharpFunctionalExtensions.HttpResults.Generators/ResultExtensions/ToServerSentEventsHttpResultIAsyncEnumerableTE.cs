namespace CSharpFunctionalExtensions.HttpResults.Generators.ResultExtensions;

internal class ToServerSentEventsHttpResultIAsyncEnumerableTE : IGenerateMethods
{
  public string Generate(string mapperClassName, string resultErrorType, string httpResultType)
  {
    return $$"""
      #if NET10_0_OR_GREATER
      /// <summary>
      /// Returns a <see cref="ServerSentEventsResult{T}" /> based of a <see cref="IAsyncEnumerable{T}" /> in case of success. Returns custom mapping in case of failure.
      /// </summary>
      public static Results<ServerSentEventsResult<T>, {{httpResultType}}> ToServerSentEventsHttpResult<T>(this Result<IAsyncEnumerable<T>,{{resultErrorType}}> result, string? eventType = null)
      {
          if (result.IsSuccess) return TypedResults.ServerSentEvents(result.Value, eventType);

          return ErrorMapperInstances.{{mapperClassName}}.Map(result.Error);
      }

      /// <summary>
      /// Returns a <see cref="ServerSentEventsResult{T}" /> based of a <see cref="IAsyncEnumerable{T}" /> in case of success. Returns custom mapping in case of failure.
      /// </summary>
      public static async Task<Results<ServerSentEventsResult<T>, {{httpResultType}}>> ToServerSentEventsHttpResult<T>(this Task<Result<IAsyncEnumerable<T>,{{resultErrorType}}>> result, string? eventType = null)
      {
          return (await result).ToServerSentEventsHttpResult(eventType);
      }
      #endif
      """;
  }
}
