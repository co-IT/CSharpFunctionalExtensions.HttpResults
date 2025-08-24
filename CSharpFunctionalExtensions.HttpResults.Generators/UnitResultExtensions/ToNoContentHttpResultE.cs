namespace CSharpFunctionalExtensions.HttpResults.Generators.UnitResultExtensions;

internal class ToNoContentHttpResultE : IGenerateMethods
{
  public string Generate(string mapperClassName, string resultErrorType, string httpResultType)
  {
    return $$"""
      /// <summary>
      /// Returns a <see cref="NoContent"/> in case of success result. Returns custom mapping in case of failure.
      /// </summary>
      public static Results<NoContent, {{httpResultType}}> ToNoContentHttpResult(this UnitResult<{{resultErrorType}}> result)
      {
          if (result.IsSuccess) return TypedResults.NoContent();

          return ErrorMapperInstances.{{mapperClassName}}.Map(result.Error);
      }

      /// <summary>
      /// Returns a <see cref="NoContent"/> in case of success result. Returns custom mapping in case of failure.
      /// </summary>
      public static async Task<Results<NoContent, {{httpResultType}}>> ToNoContentHttpResult(this Task<UnitResult<{{resultErrorType}}>> result)
      {
          return (await result).ToNoContentHttpResult();
      }
      """;
  }
}
