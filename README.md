# CSharpFunctionalExtensions.MinimalApi

[![dotnet](https://img.shields.io/badge/platform-.NET-blue)](https://www.nuget.org/packages/CSharpFunctionalExtensions.MinimalApi/)
[![nuget version](https://img.shields.io/nuget/v/CSharpFunctionalExtensions.MinimalApi)](https://www.nuget.org/packages/CSharpFunctionalExtensions.MinimalApi/)
[![nuget downloads](https://img.shields.io/nuget/dt/CSharpFunctionalExtensions.MinimalApi)](https://www.nuget.org/packages/CSharpFunctionalExtensions.MinimalApi/)
[![GitHub license](https://img.shields.io/github/license/co-IT/CSharpFunctionalExtensions.MinimalApi)](https://github.com/co-IT/CSharpFunctionalExtensions.MinimalApi/blob/main/LICENSE.md)

Extensions for [CSharpFunctionalExtensions](https://github.com/vkhorikov/CSharpFunctionalExtensions) to map Results to HttpResults in your MinimalApi

## Installation

Available on [NuGet](https://www.nuget.org/packages/CSharpFunctionalExtensions.MinimalApi/).

```bash
dotnet add package CSharpFunctionalExtensions.MinimalApi
```

or

```powershell
PM> Install-Package CSharpFunctionalExtensions.MinimalApi
```

## Usage

Available Methods:

| Method                           | Description |
|----------------------------------|-------------|
| `.ToHttpResult()`                |             |
| `.ToNoContentHttpResult()`       |             |
| `.ToCreatedHttpResult()`         |             |
| `.ToCreatedAtRouteHttpResult()`  |             |
| `.ToAcceptedHttpResult()`        |             |
| `.ToAcceptedAtRouteHttpResult()` |             |
| `.ToFileHttpResult()`            |             |
| `.ToFileStreamHttpResult()`      |             |

For almost every method you can override the default status codes for Success/Failure by passing corresponding `int` values.
