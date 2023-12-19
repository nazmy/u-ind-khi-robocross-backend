namespace domain.Identity;

public interface ILoggedInUser
{
    public Task<LoggedInUser> GetLoggedInUser();
}