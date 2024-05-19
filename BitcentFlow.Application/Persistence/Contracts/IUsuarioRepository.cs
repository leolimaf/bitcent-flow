using BitcentFlow.Domain.Models;

namespace BitcentFlow.Application.Persistence.Contracts;

public interface IUsuarioRepository
{
    Task<int> RegistrarAsync(Usuario usuario);
    Task<Usuario?> ObterPorEmailAsync(string email);
    Task<Usuario?> ObterPorIdAsync(Guid Id);
}