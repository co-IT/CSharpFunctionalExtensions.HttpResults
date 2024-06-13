using Microsoft.AspNetCore.Http;

namespace CoIt.WebApi.Middlewares;

public class TimeGeneratedHeaderMiddleware : IMiddleware
{
    public Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var headerName = "X-TimeGenerated";
        var timeGenerated = DateTimeOffset.Now.ToString("O");
        
        context.Response.Headers.Append(headerName, timeGenerated);
        
        return next(context);
    }
}