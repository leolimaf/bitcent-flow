using MyFinances.Domain.Models;

namespace MyFinances.Application.Persistence.Authentication;

public interface IUsuarioRepository
{
    Task CadastrarAsync(Usuario usuario);
    Task<bool> IsCadastradoAsync(string email, CancellationToken cancellationToken);
    Task<Usuario> ObterPorEmailAsync(string email);
    Task SalvarAlteracoesAsync(CancellationToken cancellationToken);
    Task<Usuario> ObterPorIdAsync(string id);
}