namespace CSharpFunctionalExtensions.HttpResults.Generators.ResultExtensions;

internal class ToContentHttpResultStringE : IGenerateMethods
{
  public string Generate(string mapperClassName, string resultErrorType, string httpResultType)
  {
    return $$"""
      /// <summary>
      /// Returns a <see cref="ContentHttpResult"/> in case of success result. Returns custom mapping in case of failure.
      /// </summary>
      public static Results<ContentHttpResult, {{httpResultType}}> ToContentHttpResult(this Result<string,{{resultErrorType}}> result, string? contentType = null, Encoding? contentEncoding = null, int? statusCode = null)
      {
          if (result.IsSuccess) return TypedResults.Content(result.Value, contentType, contentEncoding, statusCode);

          return ErrorMapperInstances.{{mapperClassName}}.Map(result.Error);
      }

      /// <summary>
      /// Returns a <see cref="ContentHttpResult"/> in case of success result. Returns custom mapping in case of failure.
      /// </summary>
      public static async Task<Results<ContentHttpResult, {{httpResultType}}>> ToContentHttpResult(this Task<Result<string,{{resultErrorType}}>> result, string? contentType = null, Encoding? contentEncoding = null, int? statusCode = null)
      {
          return (await result).ToContentHttpResult(contentType, contentEncoding, statusCode);
      }
      """;
  }
}
