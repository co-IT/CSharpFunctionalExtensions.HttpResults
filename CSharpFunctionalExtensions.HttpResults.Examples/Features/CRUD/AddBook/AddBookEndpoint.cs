﻿using CSharpFunctionalExtensions.HttpResults.Examples.Features.CRUD.FindBook;
using CSharpFunctionalExtensions.HttpResults.ResultExtensions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace CSharpFunctionalExtensions.HttpResults.Examples.Features.CRUD.AddBook;

public static class AddBookEndpoint
{
  public static IEndpointRouteBuilder MapAddBook(this IEndpointRouteBuilder endpointRouteBuilder)
  {
    endpointRouteBuilder.MapPost("", Handle).WithName(nameof(AddBookEndpoint));

    return endpointRouteBuilder;
  }

  private static Results<CreatedAtRoute<Book>, ProblemHttpResult> Handle(
    [FromBody] AddBookRequest request,
    [FromServices] BookService service
  )
  {
    return Book.Create(request.Title, request.Author, request.Cover)
      .Bind(book => service.Add(book))
      .ToCreatedAtRouteHttpResult(nameof(FindBookEndpoint), b => new { b.Id });
  }
}
