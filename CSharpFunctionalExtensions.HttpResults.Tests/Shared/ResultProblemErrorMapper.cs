using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CSharpFunctionalExtensions.HttpResults.Tests.Shared;

public sealed class ResultProblemErrorMapper : IResultErrorMapper<ResultProblemError, ProblemHttpResult>
{
  public ProblemHttpResult Map(ResultProblemError error) =>
    TypedResults.Problem(detail: error.Detail, statusCode: StatusCodes.Status422UnprocessableEntity);
}
