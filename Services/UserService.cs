using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using ProveedorApi.Auth;
using SecurityApi.Models;
using SecurityApi.Models.ContentResponse;
using SecurityApi.Models.ContentBody;
using SecurityApi.Models.ContentEntity;
using Microsoft.Data.SqlClient;
using static Duende.IdentityServer.Models.IdentityResources;

namespace SecurityApi.Services;

public class UserService : IUserService
{
    private readonly SecurityContext _context;
    private readonly IConfiguration _config;

    public UserService(SecurityContext context, IConfiguration configuration)
    {
        _context = context;
        _config = configuration;
    }

    public async Task<string[]> Authenticate(Authentication auth)
    {
        String[] lresult = new string[2];

        byte[] _password = (SHA256.Create()).ComputeHash(Encoding.UTF8.GetBytes(auth.password));
            
        var user = (await _context.UsuarioResponse.FromSqlInterpolated($"exec GP.get_user_details {auth.username}")
                        .ToListAsync())
                        .SingleOrDefault();
        if (user == null)
        {
            lresult[0] = "Usuario no habilitado";
            return lresult;
        }
        if (Convert.ToBase64String(user.clave) != Convert.ToBase64String(_password))
        {
            lresult[0] = "Contraseña incorrecta";
            return lresult;
        }
        
        lresult[1] = new TokenAuth(_config).Token(
            new TokenModel()
            {
                id_usuario = user.id_usuario,
                username = user.usuario,
                usercode = user.codigo_empleado,
                nombre = user.nombre,
                email = user.email,
                dni = user.dni,
                rol = user.rol,
                read_only = true
            });
           
        return lresult;
    }

    public async Task<string[]> AuthenticateResetToken(AuthUser auth)
    {
        String[] lresult = new string[2];

        var user = (await _context.UsuarioResponse.FromSqlInterpolated($"exec GP.get_user_details {auth.username}")
                        .ToListAsync())
                        .SingleOrDefault();

        if (user == null)
        {
            lresult[0] = "Usuario no habilitado";
            return lresult;
        }

        lresult[1] = new TokenAuth(_config).TokenTemp(
            new TokenModelTemp()
            {
                username = user.usuario
            });

        return lresult;
    }


    public async Task<int> ChangePassAsync(string passactual, string passnew, string usercode)
    {
        var usuario = await _context.Usuario.Where(x => x.codigo_empleado == usercode && x.estado == "S").FirstOrDefaultAsync();
        if (usuario == null) throw new Exception("No existe sus datos");

        byte[] passbyte = (SHA256.Create()).ComputeHash(Encoding.UTF8.GetBytes(passactual));
        if (Convert.ToBase64String(usuario.clave) != Convert.ToBase64String(passbyte)) throw new Exception("La contraseña actual es incorrecta");

        usuario.clave = (SHA256.Create()).ComputeHash(Encoding.UTF8.GetBytes(passnew));
        usuario.usuario_actualizacion = usercode;
        usuario.fecha_actualizacion = DateTime.Now;
        _context.Usuario.Update(usuario);
        return await _context.SaveChangesAsync();
    }

    public async Task<int> ChangePasswordUserAsync(string passnew, string usercode)
    {
        var usuario = await _context.Usuario.Where(x => x.usuario == usercode && x.estado == "S").FirstOrDefaultAsync();
        if (usuario == null) throw new Exception("No existe sus datos");

        
        usuario.clave = (SHA256.Create()).ComputeHash(Encoding.UTF8.GetBytes(passnew));
        usuario.usuario_actualizacion = usercode;
        usuario.fecha_actualizacion = DateTime.Now;
        _context.Usuario.Update(usuario);
        return await _context.SaveChangesAsync();
    }

    public async Task<UsuarioEntity> GetTokenUser(string token)
    {
        try
        {
            return await _context.Usuario.Where(x => x.token_reset == token).FirstOrDefaultAsync();
        }
        catch (System.Exception)
        {
            return null;
        }
    }

    public async Task<int> UpdateToken(UsuarioEntity usuarioEntity)
    {
        _context.Usuario.Update(usuarioEntity);
        return await _context.SaveChangesAsync();
    }
}