namespace WebApi.UseCases.Users;

public class User
{
    public required Guid Id { get; init; }
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
}