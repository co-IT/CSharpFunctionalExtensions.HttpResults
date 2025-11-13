# CSharpFunctionalExtensions.HttpResults

[![dotnet](https://img.shields.io/badge/platform-.NET-blue)](https://www.nuget.org/packages/CSharpFunctionalExtensions.HttpResults/)
[![nuget version](https://img.shields.io/nuget/v/CSharpFunctionalExtensions.HttpResults)](https://www.nuget.org/packages/CSharpFunctionalExtensions.HttpResults/)
[![nuget downloads](https://img.shields.io/nuget/dt/CSharpFunctionalExtensions.HttpResults)](https://www.nuget.org/packages/CSharpFunctionalExtensions.HttpResults/)
[![GitHub license](https://img.shields.io/github/license/co-IT/CSharpFunctionalExtensions.HttpResults)](https://github.com/co-IT/CSharpFunctionalExtensions.HttpResults/blob/main/LICENSE.md)

Seamlessly map Results from [CSharpFunctionalExtensions](https://github.com/vkhorikov/CSharpFunctionalExtensions) to HttpResults for cleaner, more fluent Web APIs

<details>
<summary><b>Table of Contents</b></summary>

1. [Overview](#overview)
2. [Installation](#installation)
3. [Usage](#usage)
1. [Available methods](#available-methods)
2. [Default mapping](#default-mapping)
3. [Custom error mapping](#custom-error-mapping)
4. [Analyzers](#analyzers)
5. [Examples](#examples)
6. [Development](#development)

</details>

## Overview

This library provides convenient extension methods to seamlessly map Results from [CSharpFunctionalExtensions](https://github.com/vkhorikov/CSharpFunctionalExtensions) to HttpResults. With this, it streamlines your Web API resulting in cleaner, more fluent code.

### Key Benefits

- ⚙️ **Zero Configuration:** Get started immediately — the mapping works out of the box without any configuration.
- 🛠️ **Customizable Mappings:** Tailor default mappings or define custom mappings for specific use cases.
- 🔗 **Fluent API:** Maintain a smooth, railway-oriented flow by chaining HttpResult mappings at the end of your Result chain.
- 🧱 **Separation of Domain and HTTP Errors:** Keeps domain errors distinct from HTTP errors, improving maintainability and clarity between business logic and web API concerns.
- ⚡ **Minimal APIs & Controllers Support:** Works with both Minimal APIs and traditional controllers in ASP.NET.
- 📦 **Full Support for ASP.NET Results:** Supports all built-in HTTP response types in ASP.NET, including `Ok`, `Created`, `NoContent`, `Accepted`, `FileStream`, and more.
- 🦺 **Typed Results:** Utilizes `TypedResults` for consistent, type-safe API responses.
- 📑 **OpenAPI Ready:** Ensures accurate OpenAPI generation for clear and reliable API documentation.
- 🛡️ **RFC Compliance:** Default mappings adhere to the RFC 9457 standard (`ProblemDetails`), ensuring your API errors are standardized and interoperable.
- 🧑‍💻 **Developer-Friendly:** Includes built-in analyzers and source generators to speed up development and reduce errors.

## Installation

Available on [NuGet](https://www.nuget.org/packages/CSharpFunctionalExtensions.HttpResults/).

```bash
dotnet add package CSharpFunctionalExtensions.HttpResults
```

or

```powershell
PM> Install-Package CSharpFunctionalExtensions.HttpResults
```

> [!NOTE]
> This library references an older version of CSharpFunctionalExtensions for wider compatibility.
> It's recommended to additionally install the latest version of CSharpFunctionalExtensions in your project to get the latest features and fixes.

## Usage

This library provides you extension methods to map the following `Result` types to `HttpResults` at the end of our result chain:

- `Result`
- `Result<T>`
- `Result<T,E>`
- `UnitResult<E>`

*Example:*

```csharp
app.MapGet("/books", (BookService service) =>
    service.Get()       //Result<Book[]>
      .ToOkHttpResult() //Results<Ok<Book[]>, ProblemHttpResult>
);
```

### Available methods

<details>
  <summary><b>Click here</b> to view all available methods.</summary>

| Method                                                   | Short Description                                                                |
|----------------------------------------------------------|----------------------------------------------------------------------------------|
| `.ToStatusCodeHttpResult()`                              | Returns `StatusCodeHttpResult` or `ProblemHttpResult`                            |
| `.ToStatusCodeHttpResult<T>()`                           | Returns `StatusCodeHttpResult` or `ProblemHttpResult`                            |
| `.ToStatusCodeHttpResult<T,E>()`                         | Returns `StatusCodeHttpResult` or custom error                                   |
| `.ToStatusCodeHttpResult<E>()`                           | Returns `StatusCodeHttpResult` or custom error                                   |
| `.ToJsonHttpResult<T>()`                                 | Returns `JsonHttpResult<T>` or `ProblemHttpResult`                               |
| `.ToJsonHttpResult<T,E>()`                               | Returns `JsonHttpResult<T>` or custom error                                      |
| `.ToOkHttpResult<T>()`                                   | Returns `Ok<T>` or `ProblemHttpResult`                                           |
| `.ToOkHttpResult<T,E>()`                                 | Returns `Ok<T>` or custom error                                                  |
| `.ToNoContentHttpResult()`                               | Returns `NoContent` or `ProblemHttpResult`                                       |
| `.ToNoContentHttpResult<T>()`                            | Discards value of `Result<T>` and returns `NoContent` or `ProblemHttpResult`     |
| `.ToNoContentHttpResult<T,E>()`                          | Discards value of `Result<T>` and returns `NoContent` or custom error            |
| `.ToNoContentHttpResult<E>()`                            | Returns `NoContent` or custom error                                              |
| `.ToCreatedHttpResult<T>()`                              | Returns `Created<T>` or `ProblemHttpResult`                                      |
| `.ToCreatedHttpResult<T,E>()`                            | Returns `Created<T>` or custom error                                             |
| `.ToCreatedAtRouteHttpResult<T>()`                       | Returns `CreatedAtRoute<T>` or `ProblemHttpResult`                               |
| `.ToCreatedAtRouteHttpResult<T,E>()`                     | Returns `CreatedAtRoute<T>` or custom error                                      |
| `.ToAcceptedHttpResult<T>()`                             | Returns `Accepted<T>` or `ProblemHttpResult`                                     |
| `.ToAcceptedHttpResult<T,E>()`                           | Returns `Accepted<T>` or custom error                                            |
| `.ToAcceptedAtRouteHttpResult<T>()`                      | Returns `AcceptedAtRoute<T>` or `ProblemHttpResult`                              |
| `.ToAcceptedAtRouteHttpResult<T,E>()`                    | Returns `AcceptedAtRoute<T>` or custom error                                     |
| `.ToFileHttpResult<byte[]>()`                            | Returns `FileContentHttpResult` or `ProblemHttpResult`                           |
| `.ToFileHttpResult<byte[],E>()`                          | Returns `FileContentHttpResult` or custom error                                  |
| `.ToFileStreamHttpResult<Stream>()`                      | Returns `FileStreamHttpResult` or `ProblemHttpResult`                            |
| `.ToFileStreamHttpResult<Stream,E>()`                    | Returns `FileStreamHttpResult` or custom error                                   |
| `.ToContentHttpResult<string>()`                         | Returns `ContentHttpResult` or `ProblemHttpResult`                               |
| `.ToContentHttpResult<string,E>()`                       | Returns `ContentHttpResult` or custom error                                      |
| `.ToServerSentEventsHttpResult<IAsyncEnumerable<T>>()`   | Returns `ServerSentEventsResult<T>` or `ProblemHttpResult` (requires >= .NET 10) |
| `.ToServerSentEventsHttpResult<IAsyncEnumerable<T>,E>()` | Returns `ServerSentEventsResult<T>` or custom error        (requires >= .NET 10) |

</details>

All methods are available in sync and async variants.

### Default mapping

By default, `Result` and `Result<T>` failures are mapped to a `ProblemHttpResult` based on [RFC9457](https://www.rfc-editor.org/rfc/rfc9457).

- The `status` property contains the status code of the HTTP response. Note: For almost every method you can override the default status codes for Success/Failure case.
- The `type` property contains a URI to the corresponding [RFC9110](https://tools.ietf.org/html/rfc9110) entry based on the status code.
- The `title` property contains a generic short messages based on the status code.
- The `detail` property contains the error string of the `Result`.

This default mapping behaviour is configured inside the [`ProblemDetailsMappingProvider`](https://github.com/co-IT/CSharpFunctionalExtensions.HttpResults/blob/main/CSharpFunctionalExtensions.HttpResults/ProblemDetailsMappingProvider.cs).

#### Override default mapping

You can override this behavior by providing your own dictionary that maps status codes to their corresponding `title` and `type` of the resulting `ProblemDetails` object.

<details>
<summary><b>Click here</b> to see an example of changing the default mapping for German localization.</summary>

```csharp
ProblemDetailsMappingProvider.DefaultMappings = new Dictionary<int, (string? Title, string? Type)>
{
  { 400, ("Ungültige Anfrage", "https://tools.ietf.org/html/rfc9110#section-15.5.1") },
  { 401, ("Nicht autorisiert", "https://tools.ietf.org/html/rfc9110#section-15.5.2") },
  { 403, ("Verboten", "https://tools.ietf.org/html/rfc9110#section-15.5.4") },
  { 404, ("Nicht gefunden", "https://tools.ietf.org/html/rfc9110#section-15.5.5") },
  { 405, ("Methode nicht erlaubt", "https://tools.ietf.org/html/rfc9110#section-15.5.6") },
  { 406, ("Nicht akzeptabel", "https://tools.ietf.org/html/rfc9110#section-15.5.7") },
  { 408, ("Zeitüberschreitung der Anfrage", "https://tools.ietf.org/html/rfc9110#section-15.5.9") },
  { 409, ("Konflikt", "https://tools.ietf.org/html/rfc9110#section-15.5.10") },
  { 412, ("Vorbedingung fehlgeschlagen", "https://tools.ietf.org/html/rfc9110#section-15.5.13") },
  { 415, ("Nicht unterstützter Medientyp", "https://tools.ietf.org/html/rfc9110#section-15.5.16") },
  { 422, ("Nicht verarbeitbare Entität", "https://tools.ietf.org/html/rfc4918#section-11.2") },
  { 426, ("Upgrade erforderlich", "https://tools.ietf.org/html/rfc9110#section-15.5.22") },
  { 500, ("Ein Fehler ist bei der Verarbeitung Ihrer Anfrage aufgetreten.", "https://tools.ietf.org/html/rfc9110#section-15.6.1") },
  { 502, ("Schlechtes Gateway", "https://tools.ietf.org/html/rfc9110#section-15.6.3") },
  { 503, ("Dienst nicht verfügbar", "https://tools.ietf.org/html/rfc9110#section-15.6.4") },
  { 504, ("Gateway-Zeitüberschreitung", "https://tools.ietf.org/html/rfc9110#section-15.6.5") },
};
```

> Example from [here](https://github.com/co-IT/CSharpFunctionalExtensions.HttpResults/blob/main/CSharpFunctionalExtensions.HttpResults.Examples/Program.cs#L9-L34)

</details>

You don't have to provide the whole dictionary; you can also override or add mappings for specific status codes like this:

```csharp
ProblemDetailsMappingProvider.AddOrUpdateMapping(420, "Enhance Your Calm", "https://http-status-code.de/420/");
```

It's recommended to override the mappings during startup e.g. in `Program.cs`.

#### Override mapping for single use case

If you need to override the mapping for a specific use case in a single location, you can provide an `Action<ProblemDetails>` to fully customize the `ProblemDetails`. This is particularly useful when you want to add extensions or tailor the `ProblemDetails` specifically for that use case.

```csharp
...
.ToOkHttpResult(customizeProblemDetails: problemDetails =>
{
  problemDetails.Title = "Custom Title";
  problemDetails.Extensions.Add("custom", "value");
});
```

### Custom error mapping

When using `Result<T,E>` or `UnitResult<E>`, this library uses a Source Generator to generate extension methods for your own custom error types.

1. Create a custom error type
    ```csharp
    public record UserNotFoundError(string UserId);
    ```
2. Create a mapper that implements `IResultErrorMapper` which maps this custom error type to an HttpResult / `Microsoft.AspNetCore.Http.IResult` that you want to return in your Web API:
    ```csharp
    public class UserNotFoundErrorMapper : IResultErrorMapper<UserNotFoundError, ProblemHttpResult>
    {
        public ProblemHttpResult Map(UserNotFoundError error)
        {
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
3. Use the auto-generated extension method:
    ```csharp
    app.MapGet("/users/{id}", (string id, UserRepository repo) =>
        repo.Find(id)       //Result<User,UserNotFoundError>
          .ToOkHttpResult() //Results<Ok<User>,ProblemHttpResult>
    );
    ```

> [!IMPORTANT]  
> Make sure that each custom error type has exactly one corresponding `IResultMapper` implementation.

> [!TIP]
> You can use the `ProblemDetailsMappingProvider.FindMapping()` method to find a suitable title and type for a status code based on [RFC9110](https://tools.ietf.org/html/rfc9110).

> [!NOTE]
> If extension methods for custom errors are missing, rebuild the project to trigger Source Generation.

## Analyzers

This library includes analyzers to help you use it correctly.

For example, they will notify you if you have multiple mappers for the same custom error type or if your mapper class doesn't have a parameterless constructor.

You can find a complete list of all analyzers [here](https://github.com/co-IT/CSharpFunctionalExtensions.HttpResults/blob/main/CSharpFunctionalExtensions.HttpResults.Generators/AnalyzerReleases.Shipped.md).

## Examples

The [`CSharpFunctionalExtensions.HttpResults.Examples`](https://github.com/co-IT/CSharpFunctionalExtensions.HttpResults/blob/main/CSharpFunctionalExtensions.HttpResults.Examples) project contains various examples demonstrating how to use this library in different scenarios, including:

- **[Basic CRUD operations](https://github.com/co-IT/CSharpFunctionalExtensions.HttpResults/blob/main/CSharpFunctionalExtensions.HttpResults.Examples/Features/CRUD)** – Handling `GET`, `POST`, `PUT`, and `DELETE` requests
- **[File handling](https://github.com/co-IT/CSharpFunctionalExtensions.HttpResults/blob/main/CSharpFunctionalExtensions.HttpResults.Examples/Features/FileStream)** – Returning files from your Web API
- **[Custom error mapping](https://github.com/co-IT/CSharpFunctionalExtensions.HttpResults/blob/main/CSharpFunctionalExtensions.HttpResults.Examples/Features/CustomError)** – Defining and mapping custom error types to meaningful HTTP responses
- **[Multiple errors in chain](https://github.com/co-IT/CSharpFunctionalExtensions.HttpResults/blob/main/CSharpFunctionalExtensions.HttpResults.Examples/Features/MultipleErrorChain)** – Using different kind of custom errors in the same result chain
- **[Customizing default mapping](https://github.com/co-IT/CSharpFunctionalExtensions.HttpResults/blob/main/CSharpFunctionalExtensions.HttpResults.Examples/Program.cs)** – Overriding default mappings for localization or specific use cases

Check out the example project for hands-on implementation details!

## Development

Contributions are welcome! Please keep the following rules in mind:

- add documentation in the form of summary comments
- add tests for your additions
- add sync and async variants where possible
- refer to existing code files and the folder structure when adding something

This project uses [CSharpier](https://csharpier.com) for code formatting. You can format your code with `dotnet csharpier .`.

### Add new extension methods

To add new methods follow these steps:

1. Add methods for `Result` and `Result<T>` to `CSharpFunctionalExtensions.HttpResults.ResultExtensions`
2. Add methods for `Result<T,E>` to `CSharpFunctionalExtensions.HttpResults.Generators.ResultExtensions` and add the class to `ResultExtensionsClassBuilder`
3. Add methods for `UnitResult<E>` to `CSharpFunctionalExtensions.HttpResults.Generators.UnitResultExtensions` and add the class to `UnitResultExtensionsClassBuilder`
4. Add tests for **all** new methods to `CSharpFunctionalExtensions.HttpResults.Tests`
5. Add methods to [README](https://github.com/co-IT/CSharpFunctionalExtensions.HttpResults/blob/main/README.md)
