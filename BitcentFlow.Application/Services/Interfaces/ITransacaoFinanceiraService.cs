using BitcentFlow.Application.DTOs.TransacaoFinanceira;
using Microsoft.AspNetCore.JsonPatch;
using Sieve.Models;

namespace BitcentFlow.Application.Services.Interfaces;

public interface ITransacaoFinanceiraService
{
    Task<ReadTransacaoDTO> AdicionarTransacao(CreateTransacaoDTO transacaoDto);
    Task<ReadTransacaoDTO> ObterTransacaoPorId(Guid id);
    Task<List<ReadTransacaoDTO>> ListarTransacoes(SieveModel model);
    Task<int> AtualizarTransacao(Guid id, UpdateTransacaoDTO transacaoDto);
    Task<int> AtualizarTransacaoParcialmente(Guid id, JsonPatchDocument transacaoDto);
    Task<int> RemoverTransacao(Guid id);
}