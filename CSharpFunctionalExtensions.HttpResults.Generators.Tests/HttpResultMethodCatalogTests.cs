using AwesomeAssertions;
using CSharpFunctionalExtensions.HttpResults.Generators.Models;
using Microsoft.CodeAnalysis.CSharp;

namespace CSharpFunctionalExtensions.HttpResults.Generators.Tests;

public class HttpResultMethodCatalogTests
{
  [Fact]
  public void Catalog_contains_every_supported_family_once()
  {
    HttpResultMethodCatalog.StringErrorMethods.Should().HaveCount(14).And.OnlyHaveUniqueItems();
    HttpResultMethodCatalog.ResultWithCustomErrorMethods.Should().HaveCount(12).And.OnlyHaveUniqueItems();
    HttpResultMethodCatalog.UnitResultWithCustomErrorMethods.Should().HaveCount(2).And.OnlyHaveUniqueItems();
  }

  [Fact]
  public void Catalog_type_and_expression_fragments_are_valid_CSharp()
  {
    var methods = HttpResultMethodCatalog
      .StringErrorMethods.Concat(HttpResultMethodCatalog.UnitResultWithCustomErrorMethods)
      .Distinct();

    foreach (var method in methods)
    {
      SyntaxFactory.ParseTypeName(method.SuccessArm).ContainsDiagnostics.Should().BeFalse();
      SyntaxFactory.ParseExpression(method.SuccessExpression).ContainsDiagnostics.Should().BeFalse();

      foreach (var parameter in method.Parameters)
        SyntaxFactory.ParseTypeName(parameter.Type).ContainsDiagnostics.Should().BeFalse();
    }
  }
}
