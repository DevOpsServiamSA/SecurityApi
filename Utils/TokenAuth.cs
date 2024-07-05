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
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                    /*[0]*/new Claim(ClaimTypes.SerialNumber, tokenModel.id_usuario.ToString()??""),
                    /*[1]*/new Claim(ClaimTypes.NameIdentifier,  tokenModel.username??""),
                    /*[2]*/new Claim(ClaimTypes.Name, tokenModel.nombre),
                    /*[4]*/new Claim(ClaimTypes.Role, tokenModel.rol),                    
                    /*[5]*/new Claim("readonly", tokenModel.read_only ? "true" : "false"),
                    /*[6]*/new Claim("hash", Guid.NewGuid().ToString()),
                    /*[7]*/new Claim("api", "security-api")
            }),
            Expires = expire_token,
            SigningCredentials = credentials
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}