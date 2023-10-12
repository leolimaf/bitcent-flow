using FluentResults;
using Microsoft.AspNetCore.Mvc;
using MinhasFinancas.API.Services.Interfaces;
using MinhasFinancas.Domain.DTOs.TransacaoFinanceira;

namespace MinhasFinancas.API.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("v{version:apiVersion}/transacoes-financeiras")]
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
    [HttpPost, Route("adicionar")]
    [ProducesResponseType(201, Type = typeof(ReadTransacaoDTO))]
    public IActionResult AdicionarTransacaoFinanceira([FromBody] CreateTransacaoDTO transacaoDto)
    {
        ReadTransacaoDTO readTransacaoDto = _transacaoFinanceiraService.AdicionarTransacao(transacaoDto);
        return CreatedAtAction(nameof(ObterTransacaoPorId), new {version = HttpContext.GetRequestedApiVersion()!.ToString(), readTransacaoDto.Id} , readTransacaoDto);
    }
    
    /// <summary>Obtem uma transação financeira</summary>
    /// <remarks>A partir do identificador de uma transação financeira do usuário, é possível obte-lá</remarks>
    /// <param name="id">Identificador da transação financeira</param>
    /// <response code="200">Requisição realizada com sucesso</response>
    /// <response code="404">A transação não foi encontrada</response>
    [HttpGet, Route("obter-id")]
    [ProducesResponseType(200, Type = typeof(ReadTransacaoDTO))]
    [ProducesResponseType(404, Type = null!)]
    public IActionResult ObterTransacaoPorId(long id)
    {
        ReadTransacaoDTO readTransacaoDto = _transacaoFinanceiraService.ObterTransacaoPorId(id);
        if (readTransacaoDto is null) 
            return NotFound(readTransacaoDto);
        return Ok(readTransacaoDto);
    }

    // TODO: Alterar endpoint p/ retornar somente as transações do usuário que estiver autenticado
    /// <summary>Lista todas as transações financeiras</summary>
    /// <remarks>Retorna todas as transações financeiras do usuário autenticado</remarks>
    /// <response code="200">Requisição realizada com sucesso</response>
    [HttpGet, Route("listar")]
    [ProducesResponseType(200, Type = typeof(List<ReadTransacaoDTO>))]
    public IActionResult ListarTransacoes()
    {
        List<ReadTransacaoDTO> readTransacaoDtos = _transacaoFinanceiraService.ListarTransacoes();
        
        return Ok(readTransacaoDtos);
    }

    /// <summary>Atualiza uma transação financeira</summary>
    /// <remarks>A partir do identificador de uma transação financeira do usuário, é possível editá-la</remarks>
    /// <param name="id">Identificador da transação financeira</param>
    /// <response code="204">Requisição realizada com sucesso</response>
    /// <response code="404">A transação não foi encontrada</response>
    [HttpPut, Route("atualizar")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404, Type = null!)]
    public IActionResult AtualizarTransacao([FromQuery] long id, [FromBody] UpdateTransacaoDTO transacaoDto)
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
    [HttpDelete, Route("remover")]
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