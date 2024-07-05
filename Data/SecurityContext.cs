
using Microsoft.EntityFrameworkCore;
using SecurityApi.Models.ContentResponse;

public class SecurityContext : DbContext {
    
    public SecurityContext(DbContextOptions<SecurityContext> options) : base(options) { }

    #region Usuario
    // public DbSet<Usuario> Usuario => Set<Usuario>();
    public DbSet<UsuarioResponse> UsuarioResponse => Set<UsuarioResponse>();
    #endregion
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        #region Store Procedure Query Select
        // modelBuilder.Entity<Usuario>().ToTable("USUARIO","GP").HasKey(x => x.id_usuario);
        #endregion
        

        #region Store Procedure Query Select
        modelBuilder.Entity<UsuarioResponse>().HasNoKey();
        #endregion
    }
}