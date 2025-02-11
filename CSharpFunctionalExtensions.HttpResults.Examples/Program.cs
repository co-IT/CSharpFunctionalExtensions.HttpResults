using CSharpFunctionalExtensions.HttpResults;
using CSharpFunctionalExtensions.HttpResults.Examples.Features.Books;
using CSharpFunctionalExtensions.HttpResults.Examples.Features.CustomError;
using CSharpFunctionalExtensions.HttpResults.Examples.Features.FileStream;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

//change default mapping to be in german
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
  {
    500,
    (
      "Ein Fehler ist bei der Verarbeitung Ihrer Anfrage aufgetreten.",
      "https://tools.ietf.org/html/rfc9110#section-15.6.1"
    )
  },
  { 502, ("Schlechtes Gateway", "https://tools.ietf.org/html/rfc9110#section-15.6.3") },
  { 503, ("Dienst nicht verfügbar", "https://tools.ietf.org/html/rfc9110#section-15.6.4") },
  { 504, ("Gateway-Zeitüberschreitung", "https://tools.ietf.org/html/rfc9110#section-15.6.5") },
};

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
