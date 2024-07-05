using System.Collections.Generic;
using System.Linq;

namespace SecurityApi.Services;

public class UserService : IUserService
{
    private readonly Dictionary<string, (string username, string[] roles)> _users = new()
    {
        { "valid-token-1", ("alice", new[] { "Admin" }) },
        { "valid-token-2", ("bob", new[] { "User" }) }
    };

    public bool ValidateToken(string token)
    {
        return _users.ContainsKey(token);
    }

    public string[] GetRoles(string token)
    {
        return _users.TryGetValue(token, out var user) ? user.roles : new string[] { };
    }
}