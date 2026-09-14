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
4. [Dependency injection](#dependency-injection)
5. [Analyzers](#analyzers)
6. [Examples](#examples)
7. [Development](#development)

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

All methods are available in sync and async variants. For custom error mappers using dependency injection - and for built-in mappings once a custom `IResultProblemDetailsProvider` exists (see [Dependency injection](#dependency-injection)) - additional overloads with a required `HttpContext` parameter are generated.

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
> Make sure that each custom error type has exactly one corresponding `IResultErrorMapper<,>` or `IServiceResultErrorMapper<,>` implementation.

> [!TIP]
> You can use the `ProblemDetailsMappingProvider.FindMapping()` method to find a suitable title and type for a status code based on [RFC9110](https://tools.ietf.org/html/rfc9110).

> [!NOTE]
> If extension methods for custom errors are missing, rebuild the project to trigger Source Generation.


## Dependency injection

All mapping methods come from an integrated source generator, so they can adapt to your project:

- Custom error mappers may opt into dependency injection by implementing `IServiceResultErrorMapper<,>` instead of `IResultErrorMapper<,>`. The generated overloads then require an `HttpContext` and resolve the mapper from `HttpContext.RequestServices` on failure — constructor injection works out of the box.
- Built-in string-error mappings (`Result`, `Result<T>`) can be overridden by registering a custom `IResultProblemDetailsProvider`. As soon as such an implementation exists in your compilation, the generator additionally emits context overloads for every built-in method.

### Setup

Call the generated zero-config registration method during startup:

```csharp
builder.Services.AddCSharpFunctionalExtensionsHttpResults();
```

This registers every valid discovered `IServiceResultErrorMapper<,>` as scoped. If exactly one valid
`IResultProblemDetailsProvider` implementation is visible, it is also registered as scoped. Without a provider,
the context-free methods keep using `ProblemDetailsMappingProvider.FindMapping()` and no provider service is registered.

### Custom error mappers with constructor injection

```csharp
public sealed class UserNotFoundErrorMapper(DocumentationLinkProvider links)
  : IServiceResultErrorMapper<UserNotFoundError, ProblemHttpResult>
{
  public ProblemHttpResult Map(UserNotFoundError error) =>
    TypedResults.Problem(
      statusCode: StatusCodes.Status404NotFound,
      type: links.For("user-not-found"),
      detail: error.Message);
}
```

Pass the `HttpContext` that Minimal APIs and controllers provide anyway:

```csharp
app.MapGet("/users/{id}", (string id, HttpContext httpContext, UserRepository repo) =>
  repo.Find(id)                     //Result<User,UserNotFoundError>
    .ToOkHttpResult(httpContext)    //Results<Ok<User>,ProblemHttpResult>
);

// Controller:
public IActionResult Get(string id) =>
  _repo.Find(id).ToOkHttpResult(HttpContext);
```

> [!IMPORTANT]
> Mappers implementing `IServiceResultErrorMapper<,>` are registered automatically when they are concrete,
> closed, accessible to generated code, and have a public constructor. Mapper implementations declared in the
> application may be internal. Mapper implementations discovered in referenced assemblies must be public so the
> consuming application's generated registration code can reference them.

### Overriding the built-in failure mapping (e.g. ProblemDetailsFactory)

Implement `IResultProblemDetailsProvider` to control how failures of `Result`/`Result<T>` are turned into
`ProblemDetails`. The library intentionally provides only the contract; framework-specific policy remains in your
application. For example, a provider can delegate to ASP.NET Core's `ProblemDetailsFactory`:

```csharp
public sealed class ProblemDetailsFactoryProvider(ProblemDetailsFactory factory)
  : IResultProblemDetailsProvider
{
  public ProblemDetails CreateProblemDetails(
    HttpContext httpContext,
    string error,
    int statusCode) =>
    factory.CreateProblemDetails(httpContext, statusCode, detail: error);
}

// ProblemDetailsFactory is supplied by MVC.
builder.Services.AddControllers();
builder.Services.AddCSharpFunctionalExtensionsHttpResults();
```

`ProblemDetailsFactory.CreateProblemDetails` creates the instance synchronously, including a custom factory's
defaults. `IProblemDetailsService` writes to the response rather than returning an instance, so it is not used by
this mapping contract.

Once exactly one valid implementation is visible in the current or a referenced assembly, every built-in method
additionally offers an overload with a required `HttpContext`, and the generated startup helper registers that
provider automatically. If the helper is not called, a context overload fails fast at runtime when its failure path
tries to resolve the provider.

```csharp
app.MapGet("/books", (HttpContext httpContext, BookService svc) =>
  svc.Get()
    .ToOkHttpResult(
      httpContext,
      failureStatusCode: 404,
      customizeProblemDetails: problemDetails =>
      {
        problemDetails.Title = "Custom Title";
        problemDetails.Extensions.Add("custom", "value");
      }));
```

The selected overload determines the base mapping: context-free overloads use the static RFC 9457 mapping, while
context overloads use the registered provider. There is no runtime fallback between the two. In both cases,
`customizeProblemDetails` runs last; this is also true for custom mappers returning `ProblemHttpResult`.

Generated registrations use `TryAddScoped`. A matching registration made before
`AddCSharpFunctionalExtensionsHttpResults()` is preserved; a matching registration made afterwards becomes the
last registration and is returned by the default `GetRequiredService` resolution. This lets applications override
provider and mapper lifetimes explicitly.

## Analyzers

This library includes analyzers to help you use it correctly:

- **CFEHTTPR002** - reported when multiple `IResultErrorMapper`/`IServiceResultErrorMapper` implementations exist for the same error type (across standard and service mappers).
- **CFEHTTPR004** - reported when a standard mapper cannot be created through an accessible parameterless constructor, including unsatisfied required members. Its code fix migrates the mapper to `IServiceResultErrorMapper<,>`; generated calls then require `HttpContext`.
- **CFEHTTPR005** - reported when more than one valid `IResultProblemDetailsProvider` is visible.
- **CFEHTTPR006** - reported for unsupported mapper shapes, such as abstract, open-generic, inaccessible, or non-DI-constructible service mappers.
- **CFEHTTPR007** - reported for unsupported provider shapes.

The analyzer package also provides a code fix for `CFEHTTPR004` that converts an `IResultErrorMapper<,>` requiring
constructor injection into an `IServiceResultErrorMapper<,>`.

The complete analyzer history is documented in the
[shipped](https://github.com/co-IT/CSharpFunctionalExtensions.HttpResults/blob/main/CSharpFunctionalExtensions.HttpResults.Generators/AnalyzerReleases.Shipped.md)
and [unshipped](https://github.com/co-IT/CSharpFunctionalExtensions.HttpResults/blob/main/CSharpFunctionalExtensions.HttpResults.Generators/AnalyzerReleases.Unshipped.md)
release files.

## Examples

The [`CSharpFunctionalExtensions.HttpResults.Examples`](https://github.com/co-IT/CSharpFunctionalExtensions.HttpResults/blob/main/CSharpFunctionalExtensions.HttpResults.Examples) project contains various examples demonstrating how to use this library in different scenarios, including:

- **[Basic CRUD operations](https://github.com/co-IT/CSharpFunctionalExtensions.HttpResults/blob/main/CSharpFunctionalExtensions.HttpResults.Examples/Features/CRUD)** – Handling `GET`, `POST`, `PUT`, and `DELETE` requests
- **[File handling](https://github.com/co-IT/CSharpFunctionalExtensions.HttpResults/blob/main/CSharpFunctionalExtensions.HttpResults.Examples/Features/FileStream)** – Returning files from your Web API
- **[Custom error mapping](https://github.com/co-IT/CSharpFunctionalExtensions.HttpResults/blob/main/CSharpFunctionalExtensions.HttpResults.Examples/Features/CustomError)** – Defining and mapping custom error types to meaningful HTTP responses
- **[Multiple errors in chain](https://github.com/co-IT/CSharpFunctionalExtensions.HttpResults/blob/main/CSharpFunctionalExtensions.HttpResults.Examples/Features/MultipleErrorChain)** – Using different kind of custom errors in the same result chain
- **[Customizing default mapping](https://github.com/co-IT/CSharpFunctionalExtensions.HttpResults/blob/main/CSharpFunctionalExtensions.HttpResults.Examples/Program.cs)** – Overriding default mappings for localization or specific use cases
- **[Dependency injection](https://github.com/co-IT/CSharpFunctionalExtensions.HttpResults/blob/main/CSharpFunctionalExtensions.HttpResults.Examples/Features/DependencyInjection)** – Service mappers with constructor injection
- **[ProblemDetails provider](https://github.com/co-IT/CSharpFunctionalExtensions.HttpResults/blob/main/CSharpFunctionalExtensions.HttpResults.Examples/Features/ProblemDetailsProvider)** – Application-owned integration of `IResultProblemDetailsProvider` with ASP.NET Core MVC

Check out the example project for hands-on implementation details!

## Development

Contributions are welcome! Please keep the following rules in mind:

- add documentation in the form of summary comments
- add tests for your additions
- add sync and async variants where possible
- refer to existing code files and the folder structure when adding something

This project uses [CSharpier](https://csharpier.com) for code formatting. You can format your code with `dotnet csharpier format .`.

### Add new extension methods

Extension methods are generated by the `CSharpFunctionalExtensions.HttpResults.Generators` project. Do not add generated mapping methods by hand to the `CSharpFunctionalExtensions.HttpResults` runtime project.

To add a method, follow these steps:

1. Add the method definition once to [`CSharpFunctionalExtensions.HttpResults.Generators/Models/HttpResultMethodCatalog.cs`](./CSharpFunctionalExtensions.HttpResults.Generators/Models/HttpResultMethodCatalog.cs), using `HttpResultMethodKind` and `ResultReceiverKind`.
2. If this is a new result family, add the kind to [`HttpResultMethodKind.cs`](./CSharpFunctionalExtensions.HttpResults.Generators/Models/HttpResultMethodKind.cs) and its method name, success result, expression, parameters, type constraints, or target-framework guard to [`HttpResultMethodDefinition.cs`](./CSharpFunctionalExtensions.HttpResults.Generators/Models/HttpResultMethodDefinition.cs).
3. If the family needs a new parameter shape, add its metadata to [`MethodParameterKind.cs`](./CSharpFunctionalExtensions.HttpResults.Generators/Models/MethodParameterKind.cs) and [`HttpResultMethodParameter.cs`](./CSharpFunctionalExtensions.HttpResults.Generators/Models/HttpResultMethodParameter.cs).
4. Add runtime tests in `CSharpFunctionalExtensions.HttpResults.Tests`: use `ResultExtensions` for built-in and `Result<T,E>` mappings, `UnitResultExtensions` for `UnitResult<E>` mappings, and `ServiceMappers/ServiceMapperFamilyTests.cs` for dependency-injection overloads. Cover sync and async variants where applicable.
5. Add generator tests in `CSharpFunctionalExtensions.HttpResults.Generators.Tests`, using `GeneratorTestHelper` to compile and assert the generated source.
6. Add tests in `CSharpFunctionalExtensions.HttpResults.IntegrationTests` when behavior depends on generated registrations, dependency injection, or the ASP.NET Core request pipeline.
7. Add the method to the **Available methods** table in this README and update the examples in `CSharpFunctionalExtensions.HttpResults.Examples` when a public usage example is useful.
