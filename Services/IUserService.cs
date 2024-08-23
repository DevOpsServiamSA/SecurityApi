using SecurityApi.Models.ContentBody;

namespace SecurityApi.Services;

public interface IUserService
{
    Task<string[]> Authenticate(Authentication auth);
    Task<int> ChangePassAsync(string passactual, string passnew, string usersession);
}
