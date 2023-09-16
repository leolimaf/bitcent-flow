using Microsoft.EntityFrameworkCore;
using MinhasFinancas.Auth.Models;

namespace MinhasFinancas.Auth.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }
    
    public DbSet<Usuario> Usuarios { get; set; }

}