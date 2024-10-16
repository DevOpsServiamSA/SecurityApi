namespace SecurityApi.Models.ContentResponse;

public class UsuarioResponse
{
    public int id_usuario { get; set; }
    public string usuario { get; set; }
    public string? nombre { get; set; }
    public string email { get; set; }
    public string codigo_empleado { get; set; }
    public string? rol { get; set; }
    public string dni { get; set; }
    public byte[] clave { get; set; }
    public string? estado { get; set; }
   /* public string? token_reset { get; set; }
    public DateTime? token_reset_expire { get; set; }
    public DateTime? token_reset_request { get; set; }*/
}
