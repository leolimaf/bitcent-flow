using BitcentFlow.Application.DTOs.TransacaoFinanceira;
using FluentResults;
using Microsoft.AspNetCore.JsonPatch;
using Sieve.Models;

namespace BitcentFlow.Application.Services.Interfaces;

public interface ITransacaoFinanceiraService
{
    ReadTransacaoDTO AdicionarTransacao(CreateTransacaoDTO transacaoDto);
    ReadTransacaoDTO ObterTransacaoPorId(Guid id);
    List<ReadTransacaoDTO> ListarTransacoes(SieveModel model);
    Result AtualizarTransacao(Guid id, UpdateTransacaoDTO transacaoDto);
    Result AtualizarTransacaoParcialmente(Guid id, JsonPatchDocument transacaoDto);
    Result RemoverTransacao(Guid id);
}