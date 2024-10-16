namespace SecurityApi.Models.ContentBody;

public class Authentication
{
    public string username { get; set; } = null!;
    public string password { get; set; } = null!;
}

public class AuthUser
{
    public string username { get; set; } = null!;
}