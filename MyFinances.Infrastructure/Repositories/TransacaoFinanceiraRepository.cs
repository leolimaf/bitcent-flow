using MyFinances.Application.Persistence;
using MyFinances.Application.Persistence.TransacaoFinanceira;
using MyFinances.Domain.Models;
using MyFinances.Infrastructure.Context;

namespace MyFinances.Infrastructure.Repositories;

public class TransacaoFinanceiraRepository : ITransacaoFinanceiraRepository
{
    private readonly AppDbContext _context;

    public TransacaoFinanceiraRepository(AppDbContext context)
    {
        _context = context;
    }

    public void Adicionar(TransacaoFinanceira transacao)
    {
        _context.Add(transacao);
        _context.SaveChanges();
    }

    public TransacaoFinanceira ObterPorId(Guid id)
    {
        return _context.TransacoesFinanceiras.Find(id)!;
    }

    public List<TransacaoFinanceira> Listar()
    {
        return _context.TransacoesFinanceiras.ToList();
    }

    public void Remover(TransacaoFinanceira transacao)
    {
        _context.TransacoesFinanceiras.Remove(transacao);
        _context.SaveChanges();
    }

    public void SalvarAlteracoes()
    {
        _context.SaveChanges();
    }
}