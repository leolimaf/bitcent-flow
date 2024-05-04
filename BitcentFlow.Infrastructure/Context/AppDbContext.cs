﻿using Microsoft.EntityFrameworkCore;
using BitcentFlow.Domain.Models;
using BitcentFlow.Infrastructure.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace BitcentFlow.Infrastructure.Context;

public class AppDbContext : IdentityDbContext<Usuario, Papel, Guid>
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }

    public DbSet<TransacaoFinanceira> TransacoesFinanceiras { get; set; }
}