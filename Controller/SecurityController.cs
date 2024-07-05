using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using SecurityApi.Models.ContentBody;
using SecurityApi.Services;

namespace SecurityApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SecurityController : ControllerBase
    {
        private readonly IUserService _userService;

        public SecurityController(IUserService userService)
        {
            _userService = userService;
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