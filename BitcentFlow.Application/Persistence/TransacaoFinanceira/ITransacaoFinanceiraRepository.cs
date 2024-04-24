namespace BitcentFlow.Application.Persistence.TransacaoFinanceira;

public interface ITransacaoFinanceiraRepository
{
    void Adicionar(Domain.Models.TransacaoFinanceira transacao);
    Domain.Models.TransacaoFinanceira ObterPorId(Guid id);
    List<Domain.Models.TransacaoFinanceira> Listar();
    void Remover(Domain.Models.TransacaoFinanceira transacao);
    void SalvarAlteracoes();
}