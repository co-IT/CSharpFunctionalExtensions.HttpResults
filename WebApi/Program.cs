using CoIt.WebApi.Middlewares;
using CSharpFunctionalExtensions;
using WebApi.CustomResultErrors.DocumentMissingResult;
using WebApi.UseCases.Users;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<UserRepository>();

builder.Services.AddSingleton<TimeGeneratedHeaderMiddleware>();
builder.Services.AddSingleton<TraceIdHeaderMiddleware>();

// Add services to the container.
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

app.MapGet("/foo", () =>
{
    return Result.Success<byte[], DocumentMissingResultError>([])
        .ToFileHttpResult();
});

app.MapGet("/foo2", () =>
{
    return Result.Success<string, DocumentMissingResultError>("test")
        .Ensure(x => x == "lol", new DocumentMissingResultError{DocumentId = Guid.NewGuid(), Details = "toll"})
        .ToHttpResult();
});

app.MapGet("/foo3", () =>
{
    return UnitResult.Success<DocumentMissingResultError>()
        .ToHttpResult();
});

app.MapGet("/users", (UserRepository repo) => TypedResults.Ok((object?)repo.GetUsers()))
    .WithName("GetUsers")
    .WithOpenApi();

app.MapGet("/user/{id:guid}", (UserRepository repo, Guid id) =>
    {
        var user = repo.GetUsers()
            .FirstOrDefault(user => user.Id == id);
        
        if (user is null)
            return Results.NotFound();
        
        return Results.Ok(user);
    })
    .WithName("GetUser")
    .WithOpenApi();

app.UseMiddleware<TimeGeneratedHeaderMiddleware>();
app.UseMiddleware<TraceIdHeaderMiddleware>();

app.Run();
