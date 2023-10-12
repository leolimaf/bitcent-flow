using Microsoft.EntityFrameworkCore;
using MinhasFinancas.Domain.Models;

namespace MinhasFinancas.Auth.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }
    
    public DbSet<Usuario> Usuarios { get; set; }

}