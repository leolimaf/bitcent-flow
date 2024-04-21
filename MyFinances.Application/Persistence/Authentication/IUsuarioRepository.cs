using MyFinances.Domain.Models;

namespace MyFinances.Application.Persistence.Authentication;

public interface IUsuarioRepository
{
    Task CadastrarAsync(Usuario? usuario);
    Task<bool> IsCadastradoAsync(string email);
    Task<Usuario> ObterPorEmailAsync(string email);
    Task SalvarAlteracoesAsync();
    Task<Usuario?> ObterPorIdAsync(string id);
}