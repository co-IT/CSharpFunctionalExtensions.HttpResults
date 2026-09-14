using System.Text;

namespace CSharpFunctionalExtensions.HttpResults.Generators.Rendering;

/// <summary>Writes the common using directives required by generated result-extension source files.</summary>
internal static class GeneratedSourceUsings
{
  public static void AppendTo(StringBuilder sourceBuilder, bool includeDependencyInjection)
  {
    sourceBuilder.AppendLine(
      """
      using CSharpFunctionalExtensions;
      using Microsoft.AspNetCore.Http;
      using Microsoft.AspNetCore.Http.HttpResults;
      using Microsoft.AspNetCore.Mvc;
      using Microsoft.Net.Http.Headers;
      using System;
      using System.Collections.Generic;
      using System.IO;
      using System.Text;
      using System.Threading.Tasks;
      """
    );

    if (includeDependencyInjection)
      sourceBuilder.AppendLine("using Microsoft.Extensions.DependencyInjection;");

    sourceBuilder.AppendLine();
  }
}
