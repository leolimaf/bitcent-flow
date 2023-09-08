using ControleFinanceiro.API.DTOs.TransacaoFinanceira;
using FluentResults;

namespace ControleFinanceiro.API.Services.Interfaces;

public interface ITransacaoFinanceiraService
{
    ReadTransacaoDto AdicionarTransacao(CreateTransacaoDto transacaoDto);
    ReadTransacaoDto ObterTransacaoPorId(long id);
    List<ReadTransacaoDto> ListarTransacoes();
    Result AtualizarTransacao(long id, UpdateTransacaoDto transacaoDto);
    Result RemoverTransacao(long id);
}