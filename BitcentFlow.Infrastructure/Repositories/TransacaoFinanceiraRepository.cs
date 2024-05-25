using BitcentFlow.Application.Persistence.Contracts;
using BitcentFlow.Domain.Models;
using BitcentFlow.Infrastructure.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace BitcentFlow.Infrastructure.Repositories;

public class TransacaoFinanceiraRepository(AppDbContext context, IHttpContextAccessor contextAccessor) : ITransacaoFinanceiraRepository
{
    private readonly string? _username = contextAccessor.HttpContext?.User.Identity?.Name;
    
    public async Task AdicionarAsync(TransacaoFinanceira transacao)
    {
        transacao.Usuario = _username;
        await context.AddAsync(transacao);
    }

    public async Task<TransacaoFinanceira?> ObterPorIdAsync(Guid id)
    {
        var transacao = await context.TransacoesFinanceiras.FindAsync(id);

        return transacao?.Usuario == _username 
            ? transacao 
            : default;
    }

    public async Task<List<TransacaoFinanceira>> ListarAsync()
    {
        return await context.TransacoesFinanceiras
            .Where(t => t.Usuario == _username)
            .ToListAsync();
    } 

    public void Remover(TransacaoFinanceira transacao)
    {
        if (transacao.Usuario != _username) 
            return;
        
        context.TransacoesFinanceiras.Remove(transacao);
    }

    public async Task<int> SalvarAlteracoesAsync()
    {
        return await context.SaveChangesAsync();
    }
}