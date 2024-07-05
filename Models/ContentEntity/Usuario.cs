namespace SecurityApi.Models.ContentEntity;

public class Usuario
{
    public int id_usuario { get; set; }
    public string usuario { get; set; }
    public string clave { get; set; }
    public string? nombre { get; set; }
    public string codigo_nombre { get; set; }
    public string? estado { get; set; }
}