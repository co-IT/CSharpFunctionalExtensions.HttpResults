# CSharpFunctionalExtensions.HttpResults

[![dotnet](https://img.shields.io/badge/platform-.NET-blue)](https://www.nuget.org/packages/CSharpFunctionalExtensions.HttpResults/)
[![nuget version](https://img.shields.io/nuget/v/CSharpFunctionalExtensions.HttpResults)](https://www.nuget.org/packages/CSharpFunctionalExtensions.HttpResults/)
[![nuget downloads](https://img.shields.io/nuget/dt/CSharpFunctionalExtensions.HttpResults)](https://www.nuget.org/packages/CSharpFunctionalExtensions.HttpResults/)
[![GitHub license](https://img.shields.io/github/license/co-IT/CSharpFunctionalExtensions.HttpResults)](https://github.com/co-IT/CSharpFunctionalExtensions.HttpResults/blob/main/LICENSE.md)

Extensions for [CSharpFunctionalExtensions](https://github.com/vkhorikov/CSharpFunctionalExtensions) to map Results to
HttpResults in your WebApi

## Overview

This library streamlines returning HttpResults from endpoints that leverage [CSharpFunctionalExtensions](https://github.com/vkhorikov/CSharpFunctionalExtensions) by offering convenient extension methods to map you result to an HttpResult.
With these, you can maintain a fluent, railway-oriented style by simply invoking the appropriate method at the end of your result chain.
It also supports custom error types and ensures a clear separation between your domain logic and API by using specific mappers to translate domain details into API responses.

## Installation

Available on [NuGet](https://www.nuget.org/packages/CSharpFunctionalExtensions.HttpResults/).

```bash
dotnet add package CSharpFunctionalExtensions.HttpResults
```

or

```powershell
PM> Install-Package CSharpFunctionalExtensions.HttpResults
```

## Usage

This library provides you extension methods to map the following types to `HttpResults`:

- `Result`
- `Result<T>`
- `Result<T,E>`
- `UnitResult<E>`

These methods are available:

| Method                                | Short Description                                                            |
|---------------------------------------|------------------------------------------------------------------------------|
| `.ToStatusCodeHttpResult()`           | Returns `StatusCodeHttpResult` or `ProblemHttpResult`                        |
| `.ToStatusCodeHttpResult<T>()`        | Returns `StatusCodeHttpResult` or `ProblemHttpResult`                        |
| `.ToStatusCodeHttpResult<T,E>()`      | Returns `StatusCodeHttpResult` or custom error                               |
| `.ToStatusCodeHttpResult<E>()`        | Returns `StatusCodeHttpResult` or custom error                               |
| `.ToJsonHttpResult<T>()`              | Returns `JsonHttpResult<T>` or `ProblemHttpResult`                           |
| `.ToJsonHttpResult<T,E>()`            | Returns `JsonHttpResult<T>` or custom error                                  |
| `.ToOkHttpResult<T>()`                | Returns `Ok<T>` or `ProblemHttpResult`                                       |
| `.ToOkHttpResult<T,E>()`              | Returns `Ok<T>` or custom error                                              |
| `.ToNoContentHttpResult()`            | Returns `NoContent` or `ProblemHttpResult`                                   |
| `.ToNoContentHttpResult<T>()`         | Discards value of `Result<T>` and returns `NoContent` or `ProblemHttpResult` |
| `.ToNoContentHttpResult<T,E>()`       | Discards value of `Result<T>` and returns `NoContent` or custom error        |
| `.ToNoContentHttpResult<E>()`         | Returns `NoContent` or custom error                                          |
| `.ToCreatedHttpResult<T>()`           | Returns `Created<T>` or `ProblemHttpResult`                                  |
| `.ToCreatedHttpResult<T,E>()`         | Returns `Created<T>` or custom error                                         |
| `.ToCreatedAtRouteHttpResult<T>()`    | Returns `CreatedAtRoute<T>` or `ProblemHttpResult`                           |
| `.ToCreatedAtRouteHttpResult<T,E>()`  | Returns `CreatedAtRoute<T>` or custom error                                  |
| `.ToAcceptedHttpResult<T>()`          | Returns `Accepted<T>` or `ProblemHttpResult`                                 |
| `.ToAcceptedHttpResult<T,E>()`        | Returns `Accepted<T>` or custom error                                        |
| `.ToAcceptedAtRouteHttpResult<T>()`   | Returns `AcceptedAtRoute<T>` or `ProblemHttpResult`                          |
| `.ToAcceptedAtRouteHttpResult<T,E>()` | Returns `AcceptedAtRoute<T>` or custom error                                 |
| `.ToFileHttpResult<byte[]>()`         | Returns `FileContentHttpResult` or `ProblemHttpResult`                       |
| `.ToFileHttpResult<byte[],E>()`       | Returns `FileContentHttpResult` or custom error                              |
| `.ToFileStreamHttpResult<Stream>()`   | Returns `FileStreamHttpResult` or `ProblemHttpResult`                        |
| `.ToFileStreamHttpResult<Stream,E>()` | Returns `FileStreamHttpResult` or custom error                               |

For almost every method you can override the default status codes for Success/Failure.

All methods are available in sync and async variants.

By default, failures get mapped to a `ProblemHttpResult` based on [RFC9457](https://www.rfc-editor.org/rfc/rfc9457).
The status property contains the status code.
The type property contains a URI to the corresponding [RFC9110](https://tools.ietf.org/html/rfc9110) entry based on the status code.
The title property contains a generic short messages based on the status code.
The detail property contains the error property of the `Result`.

If you want your own mapping logic read on.

### Custom errors

This library uses a Source Generator to generate extension methods for your own custom error types when using `Result<T,E>` or `UnitResult<E>`.

1. First create a custom error type
    ```csharp
    public class UserNotFoundError
    {
        public required string UserId { get; init; }
    }
    ```
2. Create a mapper that implements `IResultErrorMapper` which maps this custom error type to an `IResult` that you want to return in your WebAPI:
    ```csharp
    public class UserNotFoundErrorMapper : IResultErrorMapper<UserNotFoundError, ProblemHttpResult>
    {
        public Func<UserNotFoundErrorMapper, ProblemHttpResult> Map => error => {
            var problemDetails = new ProblemDetails
            {
                Status = 404,
                Title = "User not found",
                Type = "https://tools.ietf.org/html/rfc9110#section-15.5.5",
                Detail = $"The user with ID {error.UserId} couldn't be found.
            };
            
            return TypedResults.Problem(problemDetails);  
        };
    }
    ```
3. Use the generated extension method:
    ```csharp
    app.MapGet("/users/{id}", (string id) => {
        return userRepository.Find(id) //Result<User,UserNotFoundError>
            .ToOkHttpResult(); //returns 200 with User as payload or 404 with ProblemDetails object defined above
    });
    ```

Make sure that every custom error type has exactly one corresponding `IResultMapper` implementation. Otherwise, the build might fail with diagnostic error [CFEHTTPR002](./CSharpFunctionalExtensions.HttpResults.Generators/AnalyzerReleases.Shipped.md). 

If extension methods for custom errors are missing, rebuild the project to trigger Source Generation.

Optionally, there is a helper method `ProblemDetailsMap.Find()` to find a suitable title and type for a status code based on [RFC9110](https://tools.ietf.org/html/rfc9110).

## Examples

Examples are available in the [`CSharpFunctionalExtensions.HttpResults.Examples`](CSharpFunctionalExtensions.HttpResults.Examples) project.

## Development

You're welcome to contribute. Please keep these few rules in mind:

- add documentation in the form of summary comments
- add tests for your additions
- add sync and async variants where possible
- refer to existing code files and the folder structure when adding something

### Add new extension methods

To add new methods follow these steps:

1. Add methods for `Result` and `Result<T>` to `CSharpFunctionalExtensions.HttpResults.ResultExtensions`
2. Add methods for `Result<T,E>` to `CSharpFunctionalExtensions.HttpResults.Generators.ResultExtensions`
3. Add methods for `UnitResult<E>` to `CSharpFunctionalExtensions.HttpResults.Generators.UnitResultExtensions`
4. Add tests for **all** new methods to `CSharpFunctionalExtensions.HttpResults.Tests`
5. Add methods to [README](README.md)