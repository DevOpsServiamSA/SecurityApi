using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using SecurityApi.Models.ContentBody;
using SecurityApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SecurityApi.Models.ContentEntity;
using SecurityApi.Utils;
using Microsoft.Graph.ExternalConnectors;
using ProveedorApi.Auth;
using SecurityApi.Models;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;


namespace SecurityApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SecurityController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly SecurityContext _context;
        private readonly IUserService _userService;

        public SecurityController(IUserService userService, IConfiguration configuration, SecurityContext context)
        {
            _userService = userService;
            _config = configuration;
            _context = context;
        }

        [HttpPost]
        [Route("auth/token")]
        public async Task<IActionResult> PostAuth(Authentication auth)
        {
            // Validar que los parametros esten llenos
            string message = Validar(auth);
            
            if (message.Trim().Length > 0) return Ok(new { message });
            
            // Autenticar y generar el token
            string[] lAuthResult = await _userService.Authenticate(auth);
            
            if (lAuthResult.Length == 0)
            {
                return Ok(new { message = "Intente nuevamente" });
            }
            if (!string.IsNullOrEmpty(lAuthResult[0]))
            {
                return Ok(new { message = lAuthResult[0] });
            }
            if (string.IsNullOrEmpty(lAuthResult[1]))
            {
                return Ok(new { message = "Intente nuevamente" });
            }
            return Ok(new { token = lAuthResult[1] });
        }

        [HttpPost]
        [Route("requestNewPassword")]
        public async Task<ActionResult> RequestNewPassword(Empleado body)
        {
            try
            {
                var user = await new GetUserService(_context).GetItemAsync(body.identificacion);


                var connectionString = _config.GetConnectionString("Security");
                string email = null!;

                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                
                    var command = new SqlCommand("SELECT E_MAIL FROM EXACTUS.INKA.EMPLEADO WHERE IDENTIFICACION = @empleado", connection);
                    command.Parameters.AddWithValue("@empleado", body.identificacion);


                    email = (string)await command.ExecuteScalarAsync();
                
                    if (email == null)
                        return Conflict(new { msg = "No se encontró el correo electrónico para el empleado proporcionado." });
                }

                if (user == null) return Conflict(new { msg = "La información proporcionada no están en nuestro sistema." });


                var tokenModel = new TokenModelTemp() { username = body.identificacion };
                string token = new TokenAuth(_config).TokenTemp(tokenModel);

                DateTime expire_token = UtilityHelper.ExpireToken(_config["appConfig:Configuracion:ExpireResetToken"]);


                user.token_reset = token;
                user.token_reset_request = DateTime.Now;
                user.token_reset_expire = expire_token;

                int saveToken = await _userService.UpdateToken(user);
                if (saveToken == 0) return Conflict(new { msg = "No se pudo enviar registrar la solicitud. Intente nuevamente." });

                string asunto = "Solicitud de Cambio de contraseña";
                string msgCorreoHtml = "";

                //msgCorreoHtml = $@"<p>Hola {provuser.nombre},</p>";
                msgCorreoHtml += "<p>Haz solicitado actualizar sus credenciales de acceso a nuestra plataforma</p>";
                msgCorreoHtml += $@"<p>Por favor has click en el enlace";
                msgCorreoHtml += $@"<div><a href='{AppConfig.Configuracion.Website}restablecer-credencial/{token}' 
                                style='background-color: #3e3b5a;color: #fff;
                                padding: 0.6rem 0.8rem;
                                border-radius: 0.5rem;
                                text-decoration: none;border: none;
                                font-size: .9rem;'>Restablecer contraseña</a></div>";

                MailManager mail = new MailManager();
                bool status = await mail.EnviarCorreoAsync(email, asunto, msgCorreoHtml);
                if (!status) return Conflict(new { msg = $"No se pudo enviar el correo electrónico para restablecer su contraseña. Intenta nuevamente." });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return Conflict(new { msg = "No se pudo enviar la solicitud. Intente nuevamente.", error = ex.Message });
            }
            return NoContent();
        }


        [HttpPost]
        [Route("changePasswordUser")]
        public async Task<ActionResult> ChangePasswordUser(ChangePasswordUser body)
        {
            //var usercode = Request.Headers["X-User-Code"].ToString();

            if (string.IsNullOrEmpty(body.token) || string.IsNullOrEmpty(body.newPassword)
                                               || string.IsNullOrEmpty(body.confirmNewPassword))
            {
                return Conflict(new { msg = "Ingrese todos los datos solicitados" });
            }

            if (body.newPassword != body.confirmNewPassword)
            {
                return Conflict(new { msg = "La confirmación de su nueva contraseña no coincide" });
            }
                      
            try
            {
                var storedToken = await _userService.GetTokenUser(body.token);
                if (storedToken == null)
                {
                    return Conflict(new { msg = "El token es inválido o ya ha sido utilizado." });
                }

                //Desencriptar a tu pata token
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadToken(body.token) as JwtSecurityToken;
                
                if (jwtToken == null)
                {
                    return Conflict(new { msg = "Token inválido o expirado" });
                }

                //Verificar firma token
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true, // Verificar la expiración
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _config["Jwt:Issuer"],
                    ValidAudience = _config["Jwt:Audience"],
                    IssuerSigningKey = securityKey
                };

                SecurityToken validatedToken;
                var principal = handler.ValidateToken(body.token, validationParameters, out validatedToken);


                var userCode = jwtToken.Claims.First(claim => claim.Type == "username").Value;

                int statusIntChange = await _userService.ChangePasswordUserAsync(body.newPassword, userCode);
                if (statusIntChange == 0) return Conflict(new { msg = "No se pudo cambiar su contraseña." });
            }
            catch (System.Exception ex)
            {
                return Conflict(new { msg = ex.Message });
            }
        
            return NoContent();
        }


        private string Validar(Authentication auth)
        {
            if (string.IsNullOrEmpty(auth.username))
            {
                return "Ingrese su nombre usuario";
            }
            if (string.IsNullOrEmpty(auth.password))
            {
                return "Ingrese su contraseña";
            }
            return "";
        }
    }
}