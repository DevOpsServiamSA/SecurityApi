
using Microsoft.EntityFrameworkCore;
using SecurityApi.Models.ContentEntity;
using SecurityApi.Models.ContentResponse;

public class SecurityContext : DbContext {
    
    public SecurityContext(DbContextOptions<SecurityContext> options) : base(options) { }

    #region Usuario
    public DbSet<UsuarioEntity> Usuario => Set<UsuarioEntity>();
    public DbSet<UsuarioResponse> UsuarioResponse => Set<UsuarioResponse>();
    public DbSet<Empleado> Empleado => Set<Empleado>();
    #endregion
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        #region Table Transaction
        modelBuilder.Entity<UsuarioEntity>().ToTable("USUARIO", "GP").HasKey(x => x.id_usuario);
        modelBuilder.Entity<Empleado>().ToTable("EMPLEADO", "INKA").HasKey(x => x.identificacion);
        #endregion
        

        #region Store Procedure Query Select
        modelBuilder.Entity<UsuarioResponse>().HasNoKey();
        #endregion
    }
}