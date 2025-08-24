namespace CSharpFunctionalExtensions.HttpResults.Generators.ResultExtensions;

internal class ToAcceptedHttpResultTE : IGenerateMethods
{
  public string Generate(string mapperClassName, string resultErrorType, string httpResultType)
  {
    return $$"""
      /// <summary>
      /// Returns a <see cref="Accepted{TValue}"/> with Accepted status code in case of success result. Returns custom mapping in case of failure. You can provide an URI to create a location HTTP-Header.
      /// </summary>
      public static Results<Accepted<T>, {{httpResultType}}> ToAcceptedHttpResult<T>(this Result<T,{{resultErrorType}}> result, Func<T, Uri> uri)
      {
          if (result.IsSuccess) return TypedResults.Accepted(uri(result.Value), result.Value);

          return ErrorMapperInstances.{{mapperClassName}}.Map(result.Error);
      }

      /// <summary>
      /// Returns a <see cref="Accepted{TValue}"/> with Accepted status code in case of success result. Returns custom mapping in case of failure. You can provide an URI to create a location HTTP-Header.
      /// </summary>
      public static async Task<Results<Accepted<T>, {{httpResultType}}>> ToAcceptedHttpResult<T>(this Task<Result<T,{{resultErrorType}}>> result, Func<T, Uri> uri)
      {
          return (await result).ToAcceptedHttpResult(uri);
      }
      """;
  }
}
