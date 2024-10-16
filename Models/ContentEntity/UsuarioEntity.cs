using System.ComponentModel.DataAnnotations.Schema;

namespace SecurityApi.Models.ContentEntity;

public class UsuarioEntity
{
    public int id_usuario { get; set; }
    public string usuario { get; set; }
    public string? nombre { get; set; }
    public string codigo_empleado { get; set; }
    public string? estado { get; set; }
    public byte[] clave { get; set; }
    public string? usuario_creacion { get; set; }
    public DateTime? fecha_creacion { get; set; }
    public string? usuario_actualizacion { get; set; }
    public DateTime? fecha_actualizacion { get; set; }
    public string? token_reset { get; set; }
    public DateTime? token_reset_expire { get; set; }
    public DateTime? token_reset_request { get; set; }
}

public class Empleado
{
    public string? identificacion { get; set; }
}