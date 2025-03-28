﻿using CSharpFunctionalExtensions.HttpResults.ResultExtensions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace CSharpFunctionalExtensions.HttpResults.Examples.Features.CRUD.GetBooks;

public static class GetBooksEndpoint
{
  public static IEndpointRouteBuilder MapGetBooks(this IEndpointRouteBuilder endpointRouteBuilder)
  {
    endpointRouteBuilder.MapGet("", Handle).WithName(nameof(GetBooksEndpoint));

    return endpointRouteBuilder;
  }

  private static Results<Ok<List<Book>>, ProblemHttpResult> Handle([FromServices] BookService service)
  {
    return Result.Of(service.Get).ToOkHttpResult();
  }
}
