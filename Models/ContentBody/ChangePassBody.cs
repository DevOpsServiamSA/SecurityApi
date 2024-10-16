namespace SecurityApi.Models.ContentBody;

public class ChangePassBody
{
    public string? oldPassword { get; set; }
    public string? newPassword { get; set; }
    public string? confirmNewPassword { get; set; }
}

public class ChangePasswordUser
{
    public string? token { get; set; }
    public string? newPassword { get; set; }
    public string? confirmNewPassword { get; set; }
}
