using Microsoft.EntityFrameworkCore;
using MyFinances.Application.Persistence.Authentication;
using MyFinances.Domain.Models;
using MyFinances.Infrastructure.Context;

namespace MyFinances.Infrastructure.Repositories;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly AppDbContext _context;

    public UsuarioRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task CadastrarAsync(Usuario? usuario)
    {
        await _context.Usuarios.AddAsync(usuario);
    }

    public async Task<bool> IsCadastradoAsync(string email)
    {
        return await _context.Usuarios.AnyAsync(u => u.Email == email);
    }

    public async Task<Usuario> ObterPorEmailAsync(string email)
    {
        return await _context.Usuarios.SingleOrDefaultAsync(u => u.Email == email);
    }

    public async Task SalvarAlteracoesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<Usuario?> ObterPorIdAsync(string id)
    {
        return await _context.Usuarios.FindAsync(id);
    }
}