# CSharpFunctionalExtensions.HttpResults

[![dotnet](https://img.shields.io/badge/platform-.NET-blue)](https://www.nuget.org/packages/CSharpFunctionalExtensions.HttpResults/)
[![nuget version](https://img.shields.io/nuget/v/CSharpFunctionalExtensions.HttpResults)](https://www.nuget.org/packages/CSharpFunctionalExtensions.HttpResults/)
[![nuget downloads](https://img.shields.io/nuget/dt/CSharpFunctionalExtensions.HttpResults)](https://www.nuget.org/packages/CSharpFunctionalExtensions.HttpResults/)
[![GitHub license](https://img.shields.io/github/license/co-IT/CSharpFunctionalExtensions.HttpResults)](https://github.com/co-IT/CSharpFunctionalExtensions.HttpResults/blob/main/LICENSE.md)

Extensions for [CSharpFunctionalExtensions](https://github.com/vkhorikov/CSharpFunctionalExtensions) to map Results to
HttpResults in your MinimalApi

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

| Method                                | Short Description                                                                             |
|---------------------------------------|-----------------------------------------------------------------------------------------------|
| `.ToHttpResult()`                     | Returns `StatusCodeHttpResult` or `ProblemHttpResult`                                         |
| `.ToHttpResult<T>()`                  | Returns `JsonHttpResult<T>` or `ProblemHttpResult`                                            |
| `.ToHttpResult<T,E>()`                | Returns `JsonHttpResult<T>` or custom error                                                   |
| `.ToNoContentHttpResult<T>()`         | Discards value of `Result<T>` and returns empty `StatusCodeHttpResult` or `ProblemHttpResult` |
| `.ToNoContentHttpResult<T,E>()`       | Discards value of `Result<T>` and returns empty `StatusCodeHttpResult` or custom error        |
| `.ToCreatedHttpResult<T>()`           | Returns `Created<T>` or `ProblemHttpResult`                                                   |
| `.ToCreatedHttpResult<T,E>()`         | Returns `Created<T>` or custom error                                                          |
| `.ToCreatedAtRouteHttpResult<T>()`    | Returns `CreatedAtRoute<T>` or `ProblemHttpResult`                                            |
| `.ToCreatedAtRouteHttpResult<T,E>()`  | Returns `CreatedAtRoute<T>` or custom error                                                   |
| `.ToAcceptedHttpResult<T>()`          | Returns `Accepted<T>` or `ProblemHttpResult`                                                  |
| `.ToAcceptedHttpResult<T,E>()`        | Returns `Accepted<T>` or custom error                                                         |
| `.ToAcceptedAtRouteHttpResult<T>()`   | Returns `AcceptedAtRoute<T>` or `ProblemHttpResult`                                           |
| `.ToAcceptedAtRouteHttpResult<T,E>()` | Returns `AcceptedAtRoute<T>` or custom error                                                  |
| `.ToFileHttpResult<byte[]>()`         | Returns `FileContentHttpResult` or `ProblemHttpResult`                                        |
| `.ToFileHttpResult<byte[],E>()`       | Returns `FileContentHttpResult` or custom error                                               |
| `.ToFileHttpResult<Stream>()`         | Returns `FileStreamHttpResult` or `ProblemHttpResult`                                         |
| `.ToFileHttpResult<Stream,E>()`       | Returns `FileStreamHttpResult` or custom error                                                |
| `.ToFileStreamHttpResult<Stream>()`   | Returns `FileStreamHttpResult` or `ProblemHttpResult`                                         |
| `.ToFileStreamHttpResult<Stream,E>()` | Returns `FileStreamHttpResult` or custom error                                                |

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

1. First create a custom error type that implements `IResultError`
    ```csharp
    public class UserNotFoundError : IResultError
    {
        public required string UserId { get; init; }
    }
    ```
2. Create a mapper that implements `IResultErrorMapper` which maps this custom error type to another type that you want to return in your web api:
    ```csharp
    public class UserNotFoundErrorMapper : IResultErrorMapper<UserNotFoundError, Microsoft.AspNetCore.Http.IResult>
    {
        public Func<UserNotFoundErrorMapper, Microsoft.AspNetCore.Http.IResult> Map => error => {
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
        return userRepository.find(id) //Result<User,UserNotFoundError>
            .ToHttpResult();
    });
    ```

Make sure that every `IResult` implementation only has exactly one corresponding `IResultMapper` implementation.

Optionally, you can use the `ProblemDetailsMap.Find()` method to find a title and type for a status code based on [RFC9110](https://tools.ietf.org/html/rfc9110).