namespace CSharpFunctionalExtensions.HttpResults.Generators.ResultExtensions;

internal class ToFileStreamHttpResultStreamE: IGenerateMethods
{
    public string Generate(string mapperClassName, string resultErrorType, string httpResultType)
    {
        return $$"""
                 /// <summary>
                 /// Returns a <see cref="FileStreamHttpResult"/> based of a Stream in case of success result. Returns custom mapping in case of failure.
                 /// </summary>
                 public static {{httpResultType}} ToFileStreamHttpResult(this Result<Stream,{{resultErrorType}}> result, string? contentType = null,
                     string? fileDownloadName = null, DateTimeOffset? lastModified = null,
                     EntityTagHeaderValue? entityTag = null,
                     bool enableRangeProcessing = false)
                 {
                     if (result.IsSuccess) return TypedResults.Stream(result.Value, contentType, fileDownloadName, lastModified, entityTag, enableRangeProcessing);
                     
                     return new {{mapperClassName}}().Map(result.Error);
                 }
                 
                 /// <summary>
                 /// Returns a <see cref="FileStreamHttpResult"/> based of a Stream in case of success result. Returns custom mapping in case of failure.
                 /// </summary>
                 public static async Task<{{httpResultType}}> ToFileStreamHttpResult(this Task<Result<Stream,{{resultErrorType}}>> result, string? contentType = null,
                     string? fileDownloadName = null, DateTimeOffset? lastModified = null,
                     EntityTagHeaderValue? entityTag = null,
                     bool enableRangeProcessing = false)
                 {
                     return (await result).ToFileStreamHttpResult(contentType, fileDownloadName, lastModified, entityTag, enableRangeProcessing);
                 }
                 """;
    }
}