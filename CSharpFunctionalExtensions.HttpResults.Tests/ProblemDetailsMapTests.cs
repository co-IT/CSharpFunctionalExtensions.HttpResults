using FluentAssertions;

namespace CSharpFunctionalExtensions.HttpResults.Tests;

public class ProblemDetailsMapTests
{
    [Fact]
    public void Map_finds_title_and_type_for_status_code()
    {
        var problemDetails = ProblemDetailsMap.Find(400);

        problemDetails.Title.Should().NotBeEmpty();
        problemDetails.Type.Should().NotBeEmpty();
    }
    
    [Fact]
    public void Map_returns_empty_strings_for_unknown_status_code()
    {
        var problemDetails = ProblemDetailsMap.Find(888);

        problemDetails.Title.Should().BeEmpty();
        problemDetails.Type.Should().BeEmpty();
    }
}