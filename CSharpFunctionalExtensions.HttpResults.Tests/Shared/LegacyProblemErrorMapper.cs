using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CSharpFunctionalExtensions.HttpResults.Tests.Shared;

public sealed class LegacyProblemErrorMapper : IResultErrorMapper<LegacyProblemError, ProblemHttpResult>
{
  public ProblemHttpResult Map(LegacyProblemError error) =>
    TypedResults.Problem(detail: error.Detail, statusCode: StatusCodes.Status422UnprocessableEntity);
}
