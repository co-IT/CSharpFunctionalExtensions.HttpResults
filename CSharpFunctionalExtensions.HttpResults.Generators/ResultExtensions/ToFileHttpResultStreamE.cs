namespace CSharpFunctionalExtensions.HttpResults.Generators.ResultExtensions;

public partial class ResultExtensionsGenerator
{
    public static string ToFileHttpResultStreamE(string mapperClassName, string resultErrorType, string httpResultType)
    {
        return $$"""
                    public static {{httpResultType}} ToFileHttpResult(this Result<Stream,{{resultErrorType}}> result, string? contentType = null,
                     string? fileDownloadName = null, DateTimeOffset? lastModified = null,
                     EntityTagHeaderValue? entityTag = null,
                     bool enableRangeProcessing = false)
                 {
                     if (result.IsSuccess) return TypedResults.File(result.Value, contentType, fileDownloadName, lastModified, entityTag, enableRangeProcessing);
                     
                     return new {{mapperClassName}}().Map(result.Error);
                 }
                 
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