# CSharpFunctionalExtensions.HttpResults Examples

This project contains a WebApi to showcase several features of `CSharpFunctionalExtensions.HttpResults`.

## Content

### CRUD

CRUD operations of an Book store are available under [`Features/CRUD`](./Features/CRUD).

### Files

An example for a `FileContentResult` is available under [`Features/CRUD/FindBookCover/FindBookCoverEndpoint.cs`](./Features/CRUD/FindBookCover/FindBookCoverEndpoint.cs).

### Streams

An example for a `FileStreamResult` is available under [`Features/FileStream`](./Features/FileStream).

### Custom errors

An example for a custom error `AgeRestrictionError` that is used when the age validation detects an age below 18 is available under [`Features/CustomError`](./Features/CustomError).

### Dependency injection

[`Features/DependencyInjection`](./Features/DependencyInjection) shows an
`IServiceResultErrorMapper<,>` with constructor injection. Its endpoint passes the current `HttpContext` to the
generated mapping overload so the mapper can be resolved from request services.

### Problem-details provider

[`Features/ProblemDetailsProvider`](./Features/ProblemDetailsProvider) shows how an application-owned
`IResultProblemDetailsProvider` can delegate to ASP.NET Core MVC's `ProblemDetailsFactory`. The provider is
discovered and registered as scoped by `AddCSharpFunctionalExtensionsHttpResults()`; `Program.cs` calls
`AddControllers()` to register the MVC factory. The endpoint also demonstrates the per-call callback, which runs
after the provider.

## Run

You can run this project and access the OpenApi documentation under `/scalar/v1`.
