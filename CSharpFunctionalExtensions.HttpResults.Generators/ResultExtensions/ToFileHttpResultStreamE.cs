namespace CSharpFunctionalExtensions.HttpResults.Generators.ResultExtensions;

internal class ToFileHttpResultStreamE: IGenerateMethods
{
    public string Generate(string mapperClassName, string resultErrorType, string httpResultType)
    {
        return $$"""
                 /// <summary>
                 /// Returns a <see cref="FileStreamHttpResult"/> based of a byte array in case of success result. Returns custom mapping in case of failure.
                 /// </summary>
                 public static {{httpResultType}} ToFileHttpResult(this Result<Stream,{{resultErrorType}}> result, string? contentType = null,
                     string? fileDownloadName = null, DateTimeOffset? lastModified = null,
                     EntityTagHeaderValue? entityTag = null,
                     bool enableRangeProcessing = false)
                 {
                     if (result.IsSuccess) return TypedResults.File(result.Value, contentType, fileDownloadName, lastModified, entityTag, enableRangeProcessing);
                     
                     return new {{mapperClassName}}().Map(result.Error);
                 }
                 
                 /// <summary>
                 /// Returns a <see cref="FileStreamHttpResult"/> based of a byte array in case of success result. Returns custom mapping in case of failure.
                 /// </summary>
                 public static async Task<{{httpResultType}}> ToFileHttpResult(this Task<Result<Stream,{{resultErrorType}}>> result, string? contentType = null,
                     string? fileDownloadName = null, DateTimeOffset? lastModified = null,
                     EntityTagHeaderValue? entityTag = null,
                     bool enableRangeProcessing = false)
                 {
                     return (await result).ToFileHttpResult(contentType, fileDownloadName, lastModified, entityTag, enableRangeProcessing);
                 }
                 """;
    }
}