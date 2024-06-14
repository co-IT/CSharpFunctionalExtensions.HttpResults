namespace CSharpFunctionalExtensions.HttpResults.Generators.ResultExtensions;

public partial class ResultExtensionsGenerator
{
    public static string ToFileStreamHttpResultStreamE(string mapperClassName, string resultErrorType, string httpResultType)
    {
        return $$"""
                 public static {{httpResultType}} ToFileStreamHttpResult(this Result<Stream,{{resultErrorType}}> result, string? contentType = null,
                     string? fileDownloadName = null, DateTimeOffset? lastModified = null,
                     EntityTagHeaderValue? entityTag = null,
                     bool enableRangeProcessing = false)
                 {
                     if (result.IsSuccess) return TypedResults.Stream(result.Value, contentType, fileDownloadName, lastModified, entityTag, enableRangeProcessing);
                     
                     return new {{mapperClassName}}().Map(result.Error);
                 }
                 
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