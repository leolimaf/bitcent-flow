using BitcentFlow.Domain.Models;

namespace BitcentFlow.Application.Persistence.Contracts;

public interface IUsuarioRepository
{
    Task<int> RegistrarAsync(Usuario usuario);
    Task<int> LogarAsync(string email, string senhaHash);
    Task<Usuario?> ObterPorEmailAsync(string email);
    Task<Usuario?> ObterPorIdAsync(Guid Id);
}