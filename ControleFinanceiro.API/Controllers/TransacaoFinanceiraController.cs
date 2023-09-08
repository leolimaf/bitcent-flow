using ControleFinanceiro.API.DTOs.TransacaoFinanceira;
using ControleFinanceiro.API.Services.Interfaces;
using FluentResults;
using Microsoft.AspNetCore.Mvc;

namespace ControleFinanceiro.API.Controllers;

[ApiController]
[Route("transacoes-financeiras")]
[Produces("application/json")]
public class TransacaoFinanceiraController : ControllerBase
{
    private readonly ITransacaoFinanceiraService _transacaoFinanceiraService;

    public TransacaoFinanceiraController(ITransacaoFinanceiraService transacaoFinanceiraService)
    {
        _transacaoFinanceiraService = transacaoFinanceiraService;
    }

    /// <summary> Adiciona uma transação financeira</summary>
    /// <remarks>Realiza a entrada das receitas e despesas do usuário autenticado</remarks>
    /// <response code="201">Requisição realizada com sucesso</response>
    [HttpPost, Route("adicionar-transacao")]
    [ProducesResponseType(201, Type = typeof(ReadTransacaoDto))]
    public IActionResult AdicionarLivro([FromBody] CreateTransacaoDto transacaoDto)
    {
        ReadTransacaoDto readTransacaoDto = _transacaoFinanceiraService.AdicionarTransacao(transacaoDto);
        return CreatedAtAction(nameof(ObterTransacaoPorId), readTransacaoDto);
    }
    
    /// <summary>Obtem uma transação financeira</summary>
    /// <remarks>A partir do identificador de uma transação financeira do usuário, é possível obte-lá</remarks>
    /// <param name="id">Identificador da transação financeira</param>
    /// <response code="200">Requisição realizada com sucesso</response>
    /// <response code="404">A transação não foi encontrada</response>
    [HttpGet, Route("obter-transacao-por-id")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404, Type = null!)]
    public IActionResult ObterTransacaoPorId([FromQuery] long id)
    {
        ReadTransacaoDto? readTransacaoDto = _transacaoFinanceiraService.ObterTransacaoPorId(id);
        if (readTransacaoDto is null) 
            return NotFound(readTransacaoDto);
        return Ok(readTransacaoDto);
    }

    // TODO: Alterar endpoint p/ retornar somente as transações do usuário que estiver autenticado
    /// <summary>Lista todas as transações financeiras</summary>
    /// <remarks>Retorna todas as transações financeiras do usuário autenticado</remarks>
    /// <response code="200">Requisição realizada com sucesso</response>
    [HttpGet, Route("listar-transacoes")]
    [ProducesResponseType(200)]
    public IActionResult ListarTransacoes()
    {
        List<ReadTransacaoDto> readTransacaoDtos = _transacaoFinanceiraService.ListarTransacoes();
        
        return Ok(readTransacaoDtos);
    }

    /// <summary>Atualiza uma transação financeira</summary>
    /// <remarks>A partir do identificador de uma transação financeira do usuário, é possível editá-la</remarks>
    /// <param name="id">Identificador da transação financeira</param>
    /// <response code="204">Requisição realizada com sucesso</response>
    /// <response code="404">A transação não foi encontrada</response>
    [HttpPut, Route("atualizar-transacao")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404, Type = null!)]
    public IActionResult AtualizarTransacao([FromQuery] long id, [FromBody] UpdateTransacaoDto transacaoDto)
    {
        Result result = _transacaoFinanceiraService.AtualizarTransacao(id, transacaoDto);
        if (result.IsFailed)
            return NotFound();
        return NoContent();
    }

    /// <summary>Remove uma transação financeira</summary>
    /// <remarks>A partir do identificador de uma transação financeira do usuário, é possível apagá-la</remarks>
    /// <param name="id">Identificador da transação financeira</param>
    /// <response code="204">Requisição realizada com sucesso</response>
    /// <response code="404">A transação não foi encontrada</response>
    [HttpDelete, Route("remover-transacao")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404, Type = null!)]
    public IActionResult RemoverTransacao([FromQuery] long id)
    {
        Result result = _transacaoFinanceiraService.RemoverTransacao(id);
        if (result.IsFailed)
            return NotFound();
        return NoContent();
    }
}