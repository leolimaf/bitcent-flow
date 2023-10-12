using Microsoft.EntityFrameworkCore;
using MyFinances.Domain.Models;

namespace MyFinances.Auth.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }
    
    public DbSet<Usuario> Usuarios { get; set; }

}