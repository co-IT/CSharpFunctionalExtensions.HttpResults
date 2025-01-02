using CSharpFunctionalExtensions.HttpResults.Examples.Features.Books;
using CSharpFunctionalExtensions.HttpResults.Examples.Features.CustomError;
using CSharpFunctionalExtensions.HttpResults.Examples.Features.FileStream;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<BookService>();
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
  app.MapOpenApi();
  app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.MapBooksGroup();
app.MapCheckAge();
app.MapStream();

app.Run();
