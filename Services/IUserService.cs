using SecurityApi.Models.ContentBody;
using SecurityApi.Models.ContentEntity;

namespace SecurityApi.Services;

public interface IUserService
{
    Task<string[]> Authenticate(Authentication auth);
    Task<string[]> AuthenticateResetToken(AuthUser auth);
    Task<int> ChangePassAsync(string passactual, string passnew, string usersession);
    Task<int> ChangePasswordUserAsync(string passnew, string usersession);

    Task<int> UpdateToken(UsuarioEntity usuarioEntity);
    Task<UsuarioEntity> GetTokenUser(string token);
}
