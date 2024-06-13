using CSharpFunctionalExtensions.MinimalApi;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.CustomResultErrors.DocumentMissingResult;

public class DocumentMissingResultErrorMapper : IResultErrorMapper<DocumentMissingResultError, Microsoft.AspNetCore.Http.IResult>
{
    public Func<DocumentMissingResultError, Microsoft.AspNetCore.Http.IResult> Mapping => error =>
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