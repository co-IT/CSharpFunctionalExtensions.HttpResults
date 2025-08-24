namespace CSharpFunctionalExtensions.HttpResults.Generators.ResultExtensions;

internal class ToFileStreamHttpResultStreamE : IGenerateMethods
{
  public string Generate(string mapperClassName, string resultErrorType, string httpResultType)
  {
    return $$"""
      /// <summary>
      /// Returns a <see cref="FileStreamHttpResult"/> based of a Stream in case of success result. Returns custom mapping in case of failure.
      /// </summary>
      public static Results<FileStreamHttpResult, {{httpResultType}}> ToFileStreamHttpResult<T>(this Result<T,{{resultErrorType}}> result, string? contentType = null,
          string? fileDownloadName = null, DateTimeOffset? lastModified = null,
          EntityTagHeaderValue? entityTag = null,
          bool enableRangeProcessing = false) where T : Stream
      {
          if (result.IsSuccess) return TypedResults.Stream(result.Value, contentType, fileDownloadName, lastModified, entityTag, enableRangeProcessing);

          return ErrorMapperInstances.{{mapperClassName}}.Map(result.Error);
      }

      /// <summary>
      /// Returns a <see cref="FileStreamHttpResult"/> based of a Stream in case of success result. Returns custom mapping in case of failure.
      /// </summary>
      public static async Task<Results<FileStreamHttpResult, {{httpResultType}}>> ToFileStreamHttpResult<T>(this Task<Result<T,{{resultErrorType}}>> result, string? contentType = null,
          string? fileDownloadName = null, DateTimeOffset? lastModified = null,
          EntityTagHeaderValue? entityTag = null,
          bool enableRangeProcessing = false) where T : Stream
      {
          return (await result).ToFileStreamHttpResult(contentType, fileDownloadName, lastModified, entityTag, enableRangeProcessing);
      }
      """;
  }
}
