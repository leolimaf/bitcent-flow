using BitcentFlow.Domain.Entities;
using BitcentFlow.Domain.Repositories;
using BitcentFlow.Infrastructure.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace BitcentFlow.Infrastructure.Repositories;

public class TransacaoFinanceiraRepository(AppDbContext context, IHttpContextAccessor contextAccessor) : ITransacaoFinanceiraRepository
{
    private readonly string? _username = contextAccessor.HttpContext?.User.Identity?.Name;
    
    public async Task AdicionarAsync(TransacaoFinanceira transacao)
    {
        await context.AddAsync(transacao);
    }

    public async Task<TransacaoFinanceira?> ObterPorIdAsync(Guid id)
    {
        return await context.TransacoesFinanceiras.FindAsync(id);
    }

    public async Task<List<TransacaoFinanceira>> ListarAsync()
    {
        return await context.TransacoesFinanceiras
            .ToListAsync();
    } 

    public void Remover(TransacaoFinanceira transacao)
    {
        context.TransacoesFinanceiras.Remove(transacao);
    }

    public async Task<int> SalvarAlteracoesAsync()
    {
        return await context.SaveChangesAsync();
    }
}