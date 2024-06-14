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

| Method                           | Description                                      |
|----------------------------------|--------------------------------------------------|
| `.ToHttpResult()`                |                                                  |
| `.ToNoContentHttpResult()`       | Discards Result value and returns empty response |
| `.ToCreatedHttpResult()`         |                                                  |
| `.ToCreatedAtRouteHttpResult()`  |                                                  |
| `.ToAcceptedHttpResult()`        |                                                  |
| `.ToAcceptedAtRouteHttpResult()` |                                                  |
| `.ToFileHttpResult()`            |                                                  |
| `.ToFileStreamHttpResult()`      |                                                  |

For almost every method you can override the default status codes for Success/Failure by passing corresponding `int`
values.

By default, failures get mapped to a `ProblemHttpResult`.
If you want your own mapping logic or even don't want to map to `HttpErrors` read on.

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