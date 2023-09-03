using AutoMapper;
using ControleFinanceiro.API.Business.Interfaces;
using ControleFinanceiro.API.Data;
using ControleFinanceiro.API.DTOs.TransacaoFinanceira;
using ControleFinanceiro.API.Models;
using FluentResults;
using Microsoft.AspNetCore.JsonPatch;

namespace ControleFinanceiro.API.Business;

public class FinancaBusiness : IFinancaBusiness
{
    private AppDbContext _context;
    private IMapper _mapper;

    public FinancaBusiness(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public ReadTransacaoDto AdicionarTransacao(CreateTransacaoDto transacaoDto)
    {
        TransacaoFinanceira transacao = _mapper.Map<TransacaoFinanceira>(transacaoDto);
        _context.TransacoesFinanceiras.Add(transacao);
        _context.SaveChanges();
        return _mapper.Map<ReadTransacaoDto>(transacao);
    }

    public ReadTransacaoDto ObterTransacaoPorId(long id)
    {
        var transacao = _context.TransacoesFinanceiras.FirstOrDefault(t => t.Id == id);
        
        if (transacao is not null)
            return _mapper.Map<ReadTransacaoDto>(transacao);
        
        return null!;
    }

    public List<ReadTransacaoDto> ListarTransacoes()
    {
        List<TransacaoFinanceira> transacoes = _context.TransacoesFinanceiras.ToList();
        return _mapper.Map<List<ReadTransacaoDto>>(transacoes);
    }

    public Result AtualizarTransacao(long id, UpdateTransacaoDto transacaoDto)
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