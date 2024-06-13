namespace WebApi.UseCases.Users;

public class UserRepository
{
    private static readonly List<User> Users =
    [
        new User
        {
            Id = Guid.NewGuid(),
            FirstName = "Max",
            LastName = "Mustermann",
        }
    ];
    
    public IReadOnlyList<User> GetUsers() => Users;
}