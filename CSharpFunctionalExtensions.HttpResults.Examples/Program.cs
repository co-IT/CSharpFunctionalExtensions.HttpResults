using CSharpFunctionalExtensions.HttpResults.Examples.Features.Books;
using CSharpFunctionalExtensions.HttpResults.Examples.Features.CustomError;
using CSharpFunctionalExtensions.HttpResults.Examples.Features.FileStream;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<BookService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapBooksGroup();
app.MapCheckAge();
app.MapStream();

app.Run();
