using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;

namespace CSharpFunctionalExtensions.HttpResults.IntegrationTests;

/// <summary>Boots the in-memory test application for end-to-end requests.</summary>
public sealed class TestAppFactory : WebApplicationFactory<Program>
{
  protected override IHostBuilder CreateHostBuilder() => Program.CreateHostBuilder([]);
}
