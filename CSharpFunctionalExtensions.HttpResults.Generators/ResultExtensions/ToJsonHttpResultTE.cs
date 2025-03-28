﻿namespace CSharpFunctionalExtensions.HttpResults.Generators.ResultExtensions;

internal class ToJsonHttpResultTE : IGenerateMethods
{
  public string Generate(string mapperClassName, string resultErrorType, string httpResultType)
  {
    return $$"""
      /// <summary>
      /// Returns a <see cref="JsonHttpResult{TValue}"/> in case of success result. Returns custom mapping in case of failure. You can override the success status code.
      /// </summary>
      public static Results<JsonHttpResult<T>, {{httpResultType}}> ToJsonHttpResult<T>(this Result<T,{{resultErrorType}}> result, int successStatusCode = 200)
      {
          if (result.IsSuccess) return TypedResults.Json(result.Value, statusCode: successStatusCode);
          
          return new {{mapperClassName}}().Map(result.Error);
      }

      /// <summary>
      /// Returns a <see cref="JsonHttpResult{TValue}"/> in case of success result. Returns custom mapping in case of failure. You can override the success status code.
      /// </summary>
      public static async Task<Results<JsonHttpResult<T>, {{httpResultType}}>> ToJsonHttpResult<T>(this Task<Result<T,{{resultErrorType}}>> result, int successStatusCode = 200)
      {
          return (await result).ToJsonHttpResult(successStatusCode);
      }
      """;
  }
}
