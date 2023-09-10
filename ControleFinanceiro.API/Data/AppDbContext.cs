using ControleFinanceiro.API.Models;
using Microsoft.EntityFrameworkCore;

namespace ControleFinanceiro.API.Data;

public class AppDbContext : DbContext
{
    protected AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TransacaoFinanceira>()
            .HasOne(transacao => transacao.Usuario)
            .WithMany(usuario => usuario.TransacaoFinanceiras)
            .HasForeignKey(transacao => transacao.IdUsuario);
    }

    public DbSet<TransacaoFinanceira> TransacoesFinanceiras { get; set; }
    public DbSet<Usuario> Usuarios { get; set; }
}