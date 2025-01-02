# CSharpFunctionalExtensions.HttpResults Examples

This project contains a WebApi to showcase several features of `CSharpFunctionalExtensions.HttpResults`.

## Content

### CRUD

CRUD operations of an Book store are available under [`Features/Books`](./Features/Books).

### Files

An example for a `FileContentResult` is available under [`Features/Books/FindBookCover/FindBookCoverEndpoint.cs`](./Features/Books/FindBookCover/FindBookCoverEndpoint.cs).

### Streams

An example for a `FileStreamResult` is available under [`Features/FileStream`](./Features/FileStream).

### Custom errors

An example for a custom error `AgeRestrictionError` that is used when the age validation detects an age below 18 is available under [`Features/CustomError`](./Features/CustomError).

## Run

You can run this project and access the OpenApi documentation under `/scalar/v1`.
