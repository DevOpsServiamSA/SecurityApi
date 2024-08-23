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
        int roleSession = Int32.Parse(User.Claims.ToList()[4].Value);
        string rucProvSession = User.Claims.ToList()[0].Value;
        string userNameSession = User.Claims.ToList()[2].Value;

        if (string.IsNullOrEmpty(body.passactual) || string.IsNullOrEmpty(body.passnew) || string.IsNullOrEmpty(body.passnewconfirm))
        {
            return Conflict(new { msg = "Ingrese todos los datos solicitados" });
        }

        if (body.passnew != body.passnewconfirm)
        {
            return Conflict(new { msg = "La confirmación de su nueva contraseña no coincide" });
        }

        if (body.passactual == body.passnewconfirm)
        {
            return Conflict(new { msg = "su contraseña nueva tiene que ser diferente al actual" });
        }

        try
        {
            int statusIntChange = await _userService.ChangePassAsync(body.passactual, body.passnew, userNameSession);
            if (statusIntChange == 0) return Conflict(new { msg = "No se pudo cambiar su contraseña." });
        }
        catch (System.Exception ex)
        {
            return Conflict(new { msg = ex.Message });
        }

        return NoContent();
    }
}