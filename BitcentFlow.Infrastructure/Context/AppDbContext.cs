using Microsoft.EntityFrameworkCore;
using BitcentFlow.Domain.Models;

namespace BitcentFlow.Infrastructure.Context;

public class AppDbContext : DbContext
{
    public AppDbContext()
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

        modelBuilder.Entity<Usuario>()
            .HasOne(usuario => usuario.Contato)
            .WithOne(contato => contato.Usuario)
            .HasForeignKey<Usuario>(usuario => usuario.IdContato)
            .IsRequired();
    }

    public DbSet<TransacaoFinanceira> TransacoesFinanceiras { get; set; }
    public DbSet<Contato> Contatos { get; set; }
    public DbSet<Usuario?> Usuarios { get; set; }
}