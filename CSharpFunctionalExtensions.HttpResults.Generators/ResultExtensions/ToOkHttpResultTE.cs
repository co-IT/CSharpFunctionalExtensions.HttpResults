namespace CSharpFunctionalExtensions.HttpResults.Generators.ResultExtensions;

internal class ToOkHttpResultTE: IGenerateMethods
{
    public string Generate(string mapperClassName, string resultErrorType, string httpResultType)
    {
        return $$"""
                 /// <summary>
                 /// Returns a <see cref="Ok{TValue}"/> in case of success result. Returns custom mapping in case of failure.
                 /// </summary>
                 public static Results<Ok<T>, {{httpResultType}}> ToOkHttpResult<T>(this Result<T,{{resultErrorType}}> result)
                 {
                     if (result.IsSuccess) return TypedResults.Ok(result.Value);
                     
                     return new {{mapperClassName}}().Map(result.Error);
                 }
                 
                 /// <summary>
                 /// Returns a <see cref="Ok{TValue}"/> in case of success result. Returns custom mapping in case of failure.
                 /// </summary>
                 public static async Task<Results<Ok<T>, {{httpResultType}}>> ToOkHttpResult<T>(this Task<Result<T,{{resultErrorType}}>> result)
                 {
                     return (await result).ToOkHttpResult();
                 }
                 """;
    }
}