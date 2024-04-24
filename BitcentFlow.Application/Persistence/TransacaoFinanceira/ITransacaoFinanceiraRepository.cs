namespace BitcentFlow.Application.Persistence.TransacaoFinanceira;

public interface ITransacaoFinanceiraRepository
{
    Task AdicionarAsync(Domain.Models.TransacaoFinanceira transacao);
    Task<Domain.Models.TransacaoFinanceira?> ObterPorIdAsync(Guid id);
    Task<List<Domain.Models.TransacaoFinanceira>> ListarAsync();
    void Remover(Domain.Models.TransacaoFinanceira transacao);
    Task<int> SalvarAlteracoesAsync();
}