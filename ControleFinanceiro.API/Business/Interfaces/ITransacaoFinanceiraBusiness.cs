using ControleFinanceiro.API.DTOs.TransacaoFinanceira;
using FluentResults;

namespace ControleFinanceiro.API.Business.Interfaces;

public interface ITransacaoFinanceiraBusiness
{
    ReadTransacaoDto AdicionarTransacao(CreateTransacaoDto transacaoDto);
    ReadTransacaoDto ObterTransacaoPorId(long id);
    List<ReadTransacaoDto> ListarTransacoes();
    Result AtualizarTransacao(long id, UpdateTransacaoDto transacaoDto);
    Result RemoverTransacao(long id);
}