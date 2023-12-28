using Microsoft.EntityFrameworkCore;
using MyFinances.Application.Authentication.Commands.AtualizacaoToken;
using MyFinances.Application.Authentication.Common.Responses;
using MyFinances.Application.Authentication.Queries.Login;
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

    public async Task CadastrarAsync(Usuario usuario)
    {
        await _context.Usuarios.AddAsync(usuario);
    }

    public async Task<bool> IsCadastradoAsync(string email, CancellationToken cancellationToken)
    {
        return await _context.Usuarios.AnyAsync(u => u.Email == email, cancellationToken: cancellationToken);
    }

    public async Task<Usuario> ObterPorEmailAsync(string email)
    {
        return await _context.Usuarios.SingleOrDefaultAsync(u => u.Email == email);
    }

    public async Task SalvarAlteracoesAsync(CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Usuario> ObterPorIdAsync(string id)
    {
        return await _context.Usuarios.SingleOrDefaultAsync(u => u.Id.ToString() == id);
    }
}