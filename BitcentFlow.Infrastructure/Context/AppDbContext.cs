using Microsoft.EntityFrameworkCore;
using BitcentFlow.Domain.Models;

namespace BitcentFlow.Infrastructure.Context;

public class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<TransacaoFinanceira> TransacoesFinanceiras { get; set; }
}