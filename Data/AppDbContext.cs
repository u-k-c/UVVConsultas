using Microsoft.EntityFrameworkCore;
using UVVConsultas.Models;

namespace UVVConsultas.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Consulta> Consultas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasIndex(u => u.Email).IsUnique();
            entity.Property(u => u.Nome).HasMaxLength(100);
            entity.Property(u => u.Email).HasMaxLength(150);
            entity.Property(u => u.SenhaHash).HasMaxLength(255);
        });

        modelBuilder.Entity<Consulta>(entity =>
        {
            entity.Property(c => c.Especialidade).HasMaxLength(100);
            entity.Property(c => c.TipoAtendimento).HasMaxLength(80);
            entity.Property(c => c.Medico).HasMaxLength(120);
            entity.Property(c => c.Descricao).HasMaxLength(500);
            entity.HasOne(c => c.Usuario)
                .WithMany(u => u.Consultas)
                .HasForeignKey(c => c.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
