namespace CSharpFunctionalExtensions.HttpResults.Generators.ResultExtensions;

internal class ToCreatedHttpResultTE : IGenerateMethods
{
  public string Generate(string mapperClassName, string resultErrorType, string httpResultType)
  {
    return $$"""
      /// <summary>
      /// Returns a <see cref="Created{TValue}"/> with Created status code in case of success result. Returns custom mapping in case of failure. You can provide an URI to create a location HTTP-Header.
      /// </summary>
      public static Results<Created<T>, {{httpResultType}}> ToCreatedHttpResult<T>(this Result<T,{{resultErrorType}}> result, Func<T, Uri>? uri = null)
      {
          if (result.IsSuccess)
            return uri is null
              ? TypedResults.Created(string.Empty, result.Value)
              : TypedResults.Created(uri.Invoke(result.Value), result.Value);

          return new {{mapperClassName}}().Map(result.Error);
      }

      /// <summary>
      /// Returns a <see cref="Created{TValue}"/> with Created status code in case of success result. Returns custom mapping in case of failure. You can provide an URI to create a location HTTP-Header.
      /// </summary>
      public static async Task<Results<Created<T>, {{httpResultType}}>> ToCreatedHttpResult<T>(this Task<Result<T,{{resultErrorType}}>> result, Func<T, Uri>? uri = null)
      {
          return (await result).ToCreatedHttpResult(uri);
      }
      """;
  }
}
