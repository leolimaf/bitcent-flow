using System.Security.Claims;
using BitcentFlow.Application.DTOs.TransacaoFinanceira;
using BitcentFlow.Application.Persistence.TransacaoFinanceira;
using BitcentFlow.Application.Services.Interfaces;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using BitcentFlow.Domain.Exception;
using BitcentFlow.Domain.Models;
using Microsoft.EntityFrameworkCore;
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

    public async Task<ReadTransacaoDTO> AdicionarTransacao(CreateTransacaoDTO transacaoDto)
    {
        TransacaoFinanceira transacao = transacaoDto.Adapt<TransacaoFinanceira>();
        transacao.IdUsuario = _idUsuarioAutenticado;
        await _transacaoRepository.AdicionarAsync(transacao);
        await _transacaoRepository.SalvarAlteracoesAsync();
        return transacao.Adapt<ReadTransacaoDTO>();
    }

    public async Task<ReadTransacaoDTO> ObterTransacaoPorId(Guid id)
    {
        var transacao = await _transacaoRepository.ObterPorIdAsync(id);

        if (transacao is null)
            throw new BitcentFlowException(nameof(id), BitcentFlowExceptionType.NOT_FOUND, "Transação financeira não encontrada.");
            
        return transacao.Adapt<ReadTransacaoDTO>();
    }

    public async Task<List<ReadTransacaoDTO>> ListarTransacoes(SieveModel model)
    {
        var transacoes = await _transacaoRepository.ListarAsync();

        var readTransacaoDto = transacoes.Adapt<List<ReadTransacaoDTO>>().AsQueryable();
        
        readTransacaoDto = _sieveProcessor.Apply(model, readTransacaoDto);
        
        return readTransacaoDto.ToList();
    }

    public async Task<int> AtualizarTransacao(Guid id, UpdateTransacaoDTO transacaoDto)
    {
        var transacao = await _transacaoRepository.ObterPorIdAsync(id);
        
        if (transacao is null)
            throw new BitcentFlowException(nameof(id), BitcentFlowExceptionType.NOT_FOUND, "Transação financeira não encontrada.");
        
        transacao.IdUsuario = _idUsuarioAutenticado;

        transacaoDto.Adapt(transacao);
        var result = await _transacaoRepository.SalvarAlteracoesAsync();

        if(result < 1)
            throw new BitcentFlowException(nameof(id), BitcentFlowExceptionType.CONFLICT, "Falha ao atualizar transação.");
        
        return result;
    }

    public async Task<int> AtualizarTransacaoParcialmente(Guid id, JsonPatchDocument transacaoDto)
    {
        var transacao = await _transacaoRepository.ObterPorIdAsync(id);
        
        var idUsuario = _idUsuarioAutenticado;
        
        if (transacao is null)
            throw new BitcentFlowException("Transação financeira não encontrada.");
        
        transacao.IdUsuario = idUsuario;
        
        transacaoDto.ApplyTo(transacao);
       var result = await _transacaoRepository.SalvarAlteracoesAsync();
       
       if(result < 1)
           throw new BitcentFlowException(nameof(id), BitcentFlowExceptionType.CONFLICT, "Falha ao atualizar transação.");
       
        return result;
    }

    public async Task<int> RemoverTransacao(Guid id)
    {
        var transacao = await _transacaoRepository.ObterPorIdAsync(id);

        if (transacao is null)
            throw new BitcentFlowException(nameof(id), BitcentFlowExceptionType.NOT_FOUND, "Transação financeira não encontrada.");
        
        transacao.IdUsuario = _idUsuarioAutenticado;

        _transacaoRepository.Remover(transacao);
        return await _transacaoRepository.SalvarAlteracoesAsync();
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