using ControleFinanceiro.API.Business.Interfaces;
using ControleFinanceiro.API.DTOs.TransacaoFinanceira;
using FluentResults;
using Microsoft.AspNetCore.Mvc;

namespace ControleFinanceiro.API.Controllers;

[ApiController]
[Route("transacoes-financeiras")]
[Produces("application/json")]
public class TransacaoFinanceiraController : ControllerBase
{
    private readonly ITransacaoFinanceiraBusiness _transacaoFinanceiraBusiness;
    
    public TransacaoFinanceiraController(ITransacaoFinanceiraBusiness transacaoFinanceiraBusiness)
    {
        _transacaoFinanceiraBusiness = transacaoFinanceiraBusiness;
    }

    /// <remarks>Adiciona uma transação financeira do usuário.</remarks>
    /// <response code="201">Caso a inserção seja feita com sucesso, a transação é retornada.</response>
    [HttpPost, Route("adicionar-transacao")]
    [ProducesResponseType(201, Type = typeof(ReadTransacaoDto))]
    public IActionResult AdicionarLivro([FromBody] CreateTransacaoDto transacaoDto)
    {
        ReadTransacaoDto readTransacaoDto = _transacaoFinanceiraBusiness.AdicionarTransacao(transacaoDto);
        return CreatedAtAction(nameof(ObterTransacaoPorId), readTransacaoDto);
    }
    
    /// <remarks>Obtem uma transação financeira do usuário a partir do identificador da transação.</remarks>
    /// <response code="200">Caso a transação seja encontrada.</response>
    [HttpGet, Route("obter-transacao-por-id")]
    [ProducesResponseType(200)]
    public IActionResult ObterTransacaoPorId([FromQuery] long id)
    {
        ReadTransacaoDto? readTransacaoDto = _transacaoFinanceiraBusiness.ObterTransacaoPorId(id);
        if (readTransacaoDto is null) 
            return NotFound(readTransacaoDto);
        return Ok(readTransacaoDto);
    }

    // TODO: Alterar endpoint p/ retornar somente as transações do usuário que estiver autenticado
    /// <remarks>Lista todas as transações financeiras do usuário. </remarks>
    /// <response code="200">Caso o endpoint seja executado com sucesso.</response>
    [HttpGet, Route("listar-transacoes")]
    [ProducesResponseType(200)]
    public IActionResult ListarTransacoes()
    {
        List<ReadTransacaoDto> readTransacaoDtos = _transacaoFinanceiraBusiness.ListarTransacoes();
        
        return Ok(readTransacaoDtos);
    }

    /// <remarks>Atualiza uma transação financeira do usuário a partir do identificador da transação.</remarks>
    /// <response code="204">Caso a transação seja atualizada com sucesso.</response>
    [HttpPut, Route("atualizar-transacao")]
    [ProducesResponseType(204)]
    public IActionResult AtualizarTransacao([FromQuery] long id, [FromBody] UpdateTransacaoDto transacaoDto)
    {
        Result result = _transacaoFinanceiraBusiness.AtualizarTransacao(id, transacaoDto);
        if (result.IsFailed)
            return NotFound();
        return NoContent();
    }

    /// <remarks>Remove uma transação financeira do usuário a partir do identificador da transação.</remarks>
    /// <response code="204">Caso a transação seja removida com sucesso.</response>
    [HttpDelete, Route("remover-transacao")]
    [ProducesResponseType(204)]
    public IActionResult RemoverTransacao([FromQuery] long id)
    {
        Result result = _transacaoFinanceiraBusiness.RemoverTransacao(id);
        if (result.IsFailed)
            return NotFound();
        return NoContent();
    }
}