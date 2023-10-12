using FluentResults;
using MinhasFinancas.Domain.DTOs.TransacaoFinanceira;

namespace MinhasFinancas.API.Services.Interfaces;

public interface ITransacaoFinanceiraService
{
    ReadTransacaoDTO AdicionarTransacao(CreateTransacaoDTO transacaoDto);
    ReadTransacaoDTO ObterTransacaoPorId(long id);
    List<ReadTransacaoDTO> ListarTransacoes();
    Result AtualizarTransacao(long id, UpdateTransacaoDTO transacaoDto);
    Result RemoverTransacao(long id);
}