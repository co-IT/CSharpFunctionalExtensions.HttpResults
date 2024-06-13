using Microsoft.AspNetCore.Http;

namespace CoIt.WebApi.Middlewares;

public class TraceIdHeaderMiddleware : IMiddleware
{
    public Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var headerName = "X-TraceId";
        var traceId = context.TraceIdentifier;
        
        context.Response.Headers.Append(headerName, traceId);
        
        return next(context);
    }
}