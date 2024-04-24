using BitcentFlow.Application.Persistence.TransacaoFinanceira;
using BitcentFlow.Domain.Models;
using BitcentFlow.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace BitcentFlow.Infrastructure.Repositories;

public class TransacaoFinanceiraRepository(AppDbContext context) : ITransacaoFinanceiraRepository
{
    public async Task AdicionarAsync(TransacaoFinanceira transacao)
    {
        await context.AddAsync(transacao);
    }

    public async Task<TransacaoFinanceira?> ObterPorIdAsync(Guid id)
    {
        return await context.TransacoesFinanceiras.FindAsync(id)!;
    }

    public async Task<List<TransacaoFinanceira>> ListarAsync()
    {
        return await context.TransacoesFinanceiras.ToListAsync();
    } 

    public void Remover(TransacaoFinanceira transacao)
    {
        context.TransacoesFinanceiras.Remove(transacao);
        context.SaveChanges();
    }

    public async Task<int> SalvarAlteracoesAsync()
    {
        return await context.SaveChangesAsync();
    }
}