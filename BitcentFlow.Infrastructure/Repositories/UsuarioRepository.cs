using BitcentFlow.Application.Persistence.Contracts;
using BitcentFlow.Domain.Models;
using BitcentFlow.Infrastructure.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace BitcentFlow.Infrastructure.Repositories;

public class UsuarioRepository(AppDbContext context, IHttpContextAccessor contextAccessor) : IUsuarioRepository
{
    private readonly string? _username = contextAccessor.HttpContext?.User.Identity?.Name;
    
    public async Task<int> RegistrarAsync(Usuario usuario)
    {
        await context.Usuarios.AddAsync(usuario);
        return await context.SaveChangesAsync();
    }
    
    public async Task DeslogarAsync()
    {
        if (_username is not null)
        {
            var usuario = await ObterPorIdAsync(Guid.Parse(_username));
            usuario!.Token = null;
            await context.SaveChangesAsync();
        }
    }

    public async Task<Usuario?> ObterPorEmailAsync(string email)
    {
        return await context.Usuarios.FirstOrDefaultAsync(u => u!.Email == email);
    }

    public async Task<Usuario?> ObterPorIdAsync(Guid id)
    {
        return await context.Usuarios.FindAsync(id);

    }
}