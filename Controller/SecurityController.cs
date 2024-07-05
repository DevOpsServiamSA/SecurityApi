using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;
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
        public IActionResult ValidateToken(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return BadRequest("Token is required");
            }

            var isValid = _userService.ValidateToken(token);
            return Ok(isValid);
        }

        [HttpGet("roles")]
        public IActionResult GetRoles(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return BadRequest("Token is required");
            }

            var roles = _userService.GetRoles(token);
            return Ok(string.Join(",", roles));
        }
    }
}