using CSharpFunctionalExtensions.MinimalApi;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.CustomResultErrors.DocumentMissingResult;

public class DocumentMissingResultErrorMapper : IResultErrorMapper<DocumentMissingResultError, IResult>
{
    public Func<DocumentMissingResultError, IResult> Map => error =>
    {
        var problem = new ProblemDetails
        {
            Status = 404,
            Type = "https://tools.ietf.org/html/rfc9110#section-15.5.5",
            Title = "Not Found",
            Detail = error.Details
        };
        
        return TypedResults.Problem(problem);
    };
}