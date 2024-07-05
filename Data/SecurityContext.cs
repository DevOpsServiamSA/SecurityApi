
using Microsoft.EntityFrameworkCore;
using SecurityApi.Models.ContentEntity;

public class SecurityContext : DbContext {
    
    public SecurityContext(DbContextOptions<SecurityContext> options) : base(options) { }

    #region Usuario
    public DbSet<Usuario> Usuario => Set<Usuario>();
    #endregion
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Usuario>().ToTable("USUARIO","GP").HasKey(x => x.id_usuario);
    }
}