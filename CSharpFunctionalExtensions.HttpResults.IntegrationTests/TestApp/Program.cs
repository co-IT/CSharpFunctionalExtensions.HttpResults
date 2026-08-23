using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace CSharpFunctionalExtensions.HttpResults.IntegrationTests.TestApp;

/// <summary>
///   Entry point convention picked up by <see cref="WebApplicationFactory{TEntryPoint}" />.
///   Builds a minimal Web API that exercises the generated dependency-injection paths end to end.
/// </summary>
public class Program
{
  public static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args).ConfigureWebHost(webHost => webHost.UseStartup<TestAppStartup>());
}
