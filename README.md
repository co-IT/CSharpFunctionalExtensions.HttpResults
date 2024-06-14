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

For almost every method you can override the default status codes for Success/Failure by passing corresponding `int`
values.
