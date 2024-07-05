using SecurityApi.Models.ContentBody;

namespace SecurityApi.Services;

public interface IUserService
{
    Task<string[]> Authenticate(Authentication auth);
}
