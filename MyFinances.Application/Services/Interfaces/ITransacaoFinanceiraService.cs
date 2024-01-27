using FluentResults;
using Microsoft.AspNetCore.JsonPatch;
using MyFinances.Application.DTOs.TransacaoFinanceira;
using Sieve.Models;

namespace MyFinances.Application.Services.Interfaces;

public interface ITransacaoFinanceiraService
{
    ReadTransacaoDTO AdicionarTransacao(CreateTransacaoDTO transacaoDto);
    ReadTransacaoDTO ObterTransacaoPorId(Guid id);
    List<ReadTransacaoDTO> ListarTransacoes(SieveModel model);
    Result AtualizarTransacao(Guid id, UpdateTransacaoDTO transacaoDto);
    Result AtualizarTransacaoParcialmente(Guid id, JsonPatchDocument transacaoDto);
    Result RemoverTransacao(Guid id);
}