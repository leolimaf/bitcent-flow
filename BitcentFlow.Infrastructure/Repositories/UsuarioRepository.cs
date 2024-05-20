using BitcentFlow.Application.Persistence.Contracts;
using BitcentFlow.Domain.Models;
using BitcentFlow.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace BitcentFlow.Infrastructure.Repositories;

public class UsuarioRepository(AppDbContext context) : IUsuarioRepository
{
    public async Task<int> RegistrarAsync(Usuario usuario)
    {
        await context.Usuarios.AddAsync(usuario);
        return await context.SaveChangesAsync();
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