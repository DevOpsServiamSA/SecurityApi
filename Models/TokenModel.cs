namespace SecurityApi.Models;

public class TokenModel
{
    public int id_usuario { get; set; }
    public string username { get; set; } = null!;
    public string usercode { get; set; } = null!;
    public string nombre { get; set; } = null!;
    public string email { get; set; } = null!;
    public string dni { get; set; } = null!;
    public string rol { get; set; }
    public bool read_only { get; set; }
}

public class TokenModelTemp
{
    public string username { get; set; } = null!;
}