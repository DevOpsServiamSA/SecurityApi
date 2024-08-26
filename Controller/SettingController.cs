using Microsoft.AspNetCore.Mvc;
using SecurityApi.Models.ContentBody;
using SecurityApi.Services;

namespace SecurityApi.Controllers;

[ApiController]
[Route("[controller]")]
public class SettingController : ControllerBase
{
    private readonly IUserService _userService;

    public SettingController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("changepass")]
    public async Task<ActionResult> ChangePass(ChangePassBody body)
    {
        var usercode = Request.Headers["X-User-Code"].ToString();
        
        if (string.IsNullOrEmpty(usercode) || string.IsNullOrEmpty(body.oldPassword) || string.IsNullOrEmpty(body.newPassword) 
                                           || string.IsNullOrEmpty(body.confirmNewPassword))
        {
            return Conflict(new { msg = "Ingrese todos los datos solicitados" });
        }

        if (body.newPassword != body.confirmNewPassword)
        {
            return Conflict(new { msg = "La confirmación de su nueva contraseña no coincide" });
        }

        if (body.oldPassword == body.confirmNewPassword)
        {
            return Conflict(new { msg = "su contraseña nueva tiene que ser diferente al actual" });
        }

        try
        {
            int statusIntChange = await _userService.ChangePassAsync(body.oldPassword, body.newPassword, usercode);
            if (statusIntChange == 0) return Conflict(new { msg = "No se pudo cambiar su contraseña." });
        }
        catch (System.Exception ex)
        {
            return Conflict(new { msg = ex.Message });
        }

        return NoContent();
    }
}