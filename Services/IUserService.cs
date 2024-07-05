namespace SecurityApi.Services;

public interface IUserService
{
    bool ValidateToken(string token);
    string[] GetRoles(string token);
}
