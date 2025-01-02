using Microsoft.AspNetCore.Http.HttpResults;

namespace CSharpFunctionalExtensions.HttpResults.Examples.Features.CustomError;

public static class CheckAgeEndpoint
{
  public static IEndpointRouteBuilder MapCheckAge(this IEndpointRouteBuilder endpointRouteBuilder)
  {
    endpointRouteBuilder.MapPost("/check-age", Handle).WithName(nameof(CheckAgeEndpoint));

    return endpointRouteBuilder;
  }

  private static Results<NoContent, ProblemHttpResult> Handle(int age)
  {
    return Result
      .Of<int, AgeRestrictionError>(age)
      .Ensure(age => age >= 18, new AgeRestrictionError("Minimal age is 18"))
      .ToNoContentHttpResult();
  }
}
