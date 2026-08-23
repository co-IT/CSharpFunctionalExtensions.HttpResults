using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CSharpFunctionalExtensions.HttpResults.IntegrationTests.TestApp;

public sealed class TestAppStartup
{
  public TestAppStartup(IConfiguration configuration) => Configuration = configuration;

  public IConfiguration Configuration { get; }

  public void ConfigureServices(IServiceCollection services)
  {
    // Provides MVC's ProblemDetailsFactory used by the factory-backed provider below (issue #36).
    services.AddControllers();

    services.AddCSharpFunctionalExtensionsHttpResults();

    services.AddSingleton<DocumentationLinkProvider>();
  }

  public void Configure(IApplicationBuilder app)
  {
    app.UseRouting();
    app.UseEndpoints(endpoints =>
    {
      endpoints.MapDocumentEndpoints();
      endpoints.MapBookEndpoints();
    });
  }
}
