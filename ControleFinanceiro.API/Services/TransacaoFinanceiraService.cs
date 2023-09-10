using AutoMapper;
using ControleFinanceiro.API.Data;
using ControleFinanceiro.API.DTOs.TransacaoFinanceira;
using ControleFinanceiro.API.Models;
using ControleFinanceiro.API.Services.Interfaces;
using FluentResults;

namespace ControleFinanceiro.API.Services;

public class TransacaoFinanceiraService : ITransacaoFinanceiraService
{
    private AppDbContext _context;
    private IMapper _mapper;

    public TransacaoFinanceiraService(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public ReadTransacaoDTO AdicionarTransacao(CreateTransacaoDTO transacaoDto)
    {
        TransacaoFinanceira transacao = _mapper.Map<TransacaoFinanceira>(transacaoDto);
        _context.TransacoesFinanceiras.Add(transacao);
        _context.SaveChanges();
        return _mapper.Map<ReadTransacaoDTO>(transacao);
    }

    public ReadTransacaoDTO ObterTransacaoPorId(long id)
    {
        var transacao = _context.TransacoesFinanceiras.FirstOrDefault(t => t.Id == id);
        
        if (transacao is not null)
            return _mapper.Map<ReadTransacaoDTO>(transacao);
        
        return null!;
    }

    public List<ReadTransacaoDTO> ListarTransacoes()
    {
        List<TransacaoFinanceira> transacoes = _context.TransacoesFinanceiras.ToList();
        return _mapper.Map<List<ReadTransacaoDTO>>(transacoes);
    }

    public Result AtualizarTransacao(long id, UpdateTransacaoDTO transacaoDto)
    {
        var transacao = _context.TransacoesFinanceiras.FirstOrDefault(t => t.Id == id);

        if (transacao is null)
            return Result.Fail($"A transação financeira de id {id} não foi encontrada");

        _mapper.Map(transacaoDto, transacao);
        _context.SaveChanges();
        return Result.Ok();
    }

    public Result RemoverTransacao(long id)
    {
        var transacao = _context.TransacoesFinanceiras.FirstOrDefault(t => t.Id == id);

        if (transacao is null)
            return Result.Fail($"A transação financeira de id {id} não foi encontrada");

        _context.Remove(transacao);
        _context.SaveChanges();
        return Result.Ok();
    }
}