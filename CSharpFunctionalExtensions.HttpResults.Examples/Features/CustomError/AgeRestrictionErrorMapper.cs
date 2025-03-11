using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace CSharpFunctionalExtensions.HttpResults.Examples.Features.CustomError;

public class AgeRestrictionErrorMapper : IResultErrorMapper<AgeRestrictionError, ProblemHttpResult>
{
  public ProblemHttpResult Map(AgeRestrictionError error)
  {
    var problemDetails = new ProblemDetails
    {
      Title = "Age restriction violation",
      Detail = error.Message,
      Status = StatusCodes.Status400BadRequest,
      Type = ProblemDetailsMappingProvider.FindMapping(StatusCodes.Status400BadRequest).Type,
    };

    return TypedResults.Problem(problemDetails);
  }
}
