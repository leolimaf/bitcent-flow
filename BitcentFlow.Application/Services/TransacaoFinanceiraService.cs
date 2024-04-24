using System.Security.Claims;
using BitcentFlow.Application.DTOs.TransacaoFinanceira;
using BitcentFlow.Application.Persistence.TransacaoFinanceira;
using BitcentFlow.Application.Services.Interfaces;
using FluentResults;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using BitcentFlow.Domain.Exception;
using BitcentFlow.Domain.Models;
using Sieve.Models;
using Sieve.Services;

namespace BitcentFlow.Application.Services;

public class TransacaoFinanceiraService : ITransacaoFinanceiraService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly SieveProcessor _sieveProcessor;
    private readonly Guid _idUsuarioAutenticado;
    private readonly ITransacaoFinanceiraRepository _transacaoRepository;


    public TransacaoFinanceiraService(ITransacaoFinanceiraRepository transacaoRepository, IHttpContextAccessor httpContextAccessor, SieveProcessor sieveProcessor)
    {
        _httpContextAccessor = httpContextAccessor;
        _sieveProcessor = sieveProcessor;
        _transacaoRepository = transacaoRepository;
        _idUsuarioAutenticado = ObterIdDoUsuarioAutenticado();
    }

    public ReadTransacaoDTO AdicionarTransacao(CreateTransacaoDTO transacaoDto)
    {
        TransacaoFinanceira transacao = transacaoDto.Adapt<TransacaoFinanceira>();
        transacao.IdUsuario = _idUsuarioAutenticado;
        _transacaoRepository.Adicionar(transacao);
        _transacaoRepository.SalvarAlteracoes();
        return transacao.Adapt<ReadTransacaoDTO>();
    }

    public ReadTransacaoDTO ObterTransacaoPorId(Guid id)
    {
        var transacao = _transacaoRepository.ObterPorId(id);

        if (transacao is null)
            throw new BitcentFlowException(nameof(id), BitcentFlowExceptionType.NOT_FOUND, "Transação financeira não encontrada.");
            
        return transacao.Adapt<ReadTransacaoDTO>();
    }

    public List<ReadTransacaoDTO> ListarTransacoes(SieveModel model)
    {
        var transacoes = _transacaoRepository.Listar();

        var readTransacaoDto = transacoes.Adapt<List<ReadTransacaoDTO>>().AsQueryable();
        
        readTransacaoDto = _sieveProcessor.Apply(model, readTransacaoDto);
        
        return readTransacaoDto.ToList();
    }

    public Result AtualizarTransacao(Guid id, UpdateTransacaoDTO transacaoDto)
    {
        var transacao = _transacaoRepository.ObterPorId(id);
        
        if (transacao is null)
            throw new BitcentFlowException(nameof(id), BitcentFlowExceptionType.NOT_FOUND, "Transação financeira não encontrada.");
        
        transacao.IdUsuario = _idUsuarioAutenticado;

        transacaoDto.Adapt(transacao);
        _transacaoRepository.SalvarAlteracoes();
        return Result.Ok();
    }

    public Result AtualizarTransacaoParcialmente(Guid id, JsonPatchDocument transacaoDto)
    {
        var transacao = _transacaoRepository.ObterPorId(id);
        
        var idUsuario = _idUsuarioAutenticado;
        
        if (transacao is null)
            throw new BitcentFlowException("Transação financeira não encontrada.");
        
        transacao.IdUsuario = idUsuario;
        
        transacaoDto.ApplyTo(transacao);
        _transacaoRepository.SalvarAlteracoes();
        return Result.Ok();
    }

    public Result RemoverTransacao(Guid id)
    {
        var transacao = _transacaoRepository.ObterPorId(id);

        if (transacao is null)
            throw new BitcentFlowException(nameof(id), BitcentFlowExceptionType.NOT_FOUND, "Transação financeira não encontrada.");
        
        transacao.IdUsuario = _idUsuarioAutenticado;

        _transacaoRepository.Remover(transacao);
        _transacaoRepository.SalvarAlteracoes();
        return Result.Ok();
    }
    
    private Guid ObterIdDoUsuarioAutenticado()
    {
        var idUsuario = _httpContextAccessor.HttpContext?.User.Claims
            .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        if (idUsuario is null)
            throw new BitcentFlowException(nameof(idUsuario), BitcentFlowExceptionType.UNAUTHORIZED);
        
        return new Guid(idUsuario);
    }
}