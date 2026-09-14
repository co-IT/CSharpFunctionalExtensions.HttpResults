using CSharpFunctionalExtensions.HttpResults.IntegrationTests.Features.Books;
using CSharpFunctionalExtensions.HttpResults.IntegrationTests.Features.Documents;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CSharpFunctionalExtensions.HttpResults.IntegrationTests;

/// <summary>
///   Entry point convention picked up by <see cref="WebApplicationFactory{TEntryPoint}" />.
///   Builds a minimal Web API that exercises the generated dependency-injection paths end to end.
/// </summary>
public class Program
{
  public static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
      .ConfigureWebHost(webHost =>
        webHost
          .ConfigureServices(services =>
          {
            // Provides MVC's ProblemDetailsFactory used by the factory-backed provider below (issue #36).
            services.AddControllers();
            services.AddCSharpFunctionalExtensionsHttpResults();
            services.AddSingleton<DocumentationLinkProvider>();
          })
          .Configure(app =>
          {
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
              endpoints.MapDocumentEndpoints();
              endpoints.MapBookEndpoints();
            });
          })
      );
}
