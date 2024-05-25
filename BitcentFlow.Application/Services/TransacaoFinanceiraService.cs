using BitcentFlow.Application.DTOs.TransacaoFinanceira;
using BitcentFlow.Application.Persistence.Contracts;
using BitcentFlow.Application.Services.Contracts;
using Mapster;
using Microsoft.AspNetCore.JsonPatch;
using BitcentFlow.Domain.Exception;
using BitcentFlow.Domain.Models;
using Sieve.Models;
using Sieve.Services;

namespace BitcentFlow.Application.Services;

public class TransacaoFinanceiraService : ITransacaoFinanceiraService
{
    private readonly SieveProcessor _sieveProcessor;
    private readonly ITransacaoFinanceiraRepository _transacaoRepository;
    
    public TransacaoFinanceiraService(ITransacaoFinanceiraRepository transacaoRepository, SieveProcessor sieveProcessor)
    {
        _sieveProcessor = sieveProcessor;
        _transacaoRepository = transacaoRepository;
    }

    public async Task<ReadTransacaoDTO> AdicionarTransacao(CreateTransacaoDTO transacaoDto)
    {
        TransacaoFinanceira transacao = transacaoDto.Adapt<TransacaoFinanceira>();
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

        transacaoDto.Adapt(transacao);
        var result = await _transacaoRepository.SalvarAlteracoesAsync();

        if(result < 1)
            throw new BitcentFlowException(nameof(id), BitcentFlowExceptionType.CONFLICT, "Falha ao atualizar transação.");
        
        return result;
    }

    public async Task<int> AtualizarTransacaoParcialmente(Guid id, JsonPatchDocument transacaoDto)
    {
        var transacao = await _transacaoRepository.ObterPorIdAsync(id);
        
        if (transacao is null)
            throw new BitcentFlowException("Transação financeira não encontrada.");
        
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

        _transacaoRepository.Remover(transacao);
        return await _transacaoRepository.SalvarAlteracoesAsync();
    }
}