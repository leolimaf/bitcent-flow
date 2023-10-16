using FluentResults;
using MyFinances.Domain.DTOs.TransacaoFinanceira;

namespace MyFinances.API.Services.Interfaces;

public interface ITransacaoFinanceiraService
{
    ReadTransacaoDTO AdicionarTransacao(CreateTransacaoDTO transacaoDto);
    ReadTransacaoDTO ObterTransacaoPorId(Guid id);
    List<ReadTransacaoDTO> ListarTransacoes();
    Result AtualizarTransacao(Guid id, UpdateTransacaoDTO transacaoDto);
    Result RemoverTransacao(Guid id);
}