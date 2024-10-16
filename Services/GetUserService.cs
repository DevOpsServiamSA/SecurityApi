using Microsoft.EntityFrameworkCore;
using SecurityApi.Models.ContentEntity;

namespace SecurityApi.Services
{
    public class GetUserService
    {
        private readonly SecurityContext _context;
        public GetUserService(SecurityContext context)
        {
            _context = context;
        }
        public async Task<UsuarioEntity?> GetItemAsync(string usuario)
        {
            try
            {
                return await _context.Usuario.Where(x => x.usuario == usuario).FirstOrDefaultAsync();
            }
            catch (System.Exception)
            {
                return null;
            }
        }
    }
}
