using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using SecurityApi.Models;

namespace ProveedorApi.Auth;

public class TokenAuth
{
    private readonly IConfiguration _config;
    public TokenAuth(IConfiguration config)
    {
        _config = config;
    }

    public string Token(TokenModel tokenModel)
    {
        DateTime expire_token = UtilityHelper.ExpireToken(_config["ExpiresToken"]);
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var issuer = _config["Jwt:Issuer"];
        var audience = _config["Jwt:Audience"];
        var subject = new List<Claim>
        {
            /*[0]*/new Claim(ClaimTypes.SerialNumber, tokenModel.id_usuario.ToString()??""), //ID
            /*[1]*/new Claim("username",  tokenModel.username??""), //USERNAME
            /*[1]*/new Claim("usercode",  tokenModel.usercode??""), //USERNAME
            /*[2]*/new Claim("nombre", tokenModel.nombre), //NOMBRE
            /*[3]*/new Claim("rol", tokenModel.rol),       
            /*[3]*/new Claim("email", tokenModel.email),  
            /*[3]*/new Claim("dni", tokenModel.dni),  
            /*[4]*/new Claim("readonly", tokenModel.read_only ? "true" : "false"),
            /*[5]*/new Claim("hash", Guid.NewGuid().ToString()),
            /*[6]*/new Claim("api", "security-api")
        };
        
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = issuer,
            Audience = audience,
            Subject = new ClaimsIdentity(subject.ToArray()),
            Expires = expire_token,
            SigningCredentials = credentials
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}