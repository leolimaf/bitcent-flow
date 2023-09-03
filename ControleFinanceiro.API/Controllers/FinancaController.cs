using ControleFinanceiro.API.Business.Interfaces;
using ControleFinanceiro.API.DTOs.TransacaoFinanceira;
using FluentResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace ControleFinanceiro.API.Controllers;

[ApiController]
[Route("transacoes-financeiras")]
public class FinancaController : ControllerBase
{
    private readonly IFinancaBusiness _financaBusiness;
    
    public FinancaController(IFinancaBusiness financaBusiness)
    {
        _financaBusiness = financaBusiness;
    }

    [HttpPost, Route("adicionar-transacao")]
    public IActionResult AdicionarLivro([FromBody] CreateTransacaoDto transacaoDto)
    {
        ReadTransacaoDto readTransacaoDto = _financaBusiness.AdicionarTransacao(transacaoDto);
        return CreatedAtAction(nameof(ObterTransacaoPorId), new {readTransacaoDto.Id});
    }
    
    [HttpGet, Route("obter-transacao-por-id")]
    public IActionResult ObterTransacaoPorId([FromQuery] long id)
    {
        ReadTransacaoDto? readTransacaoDto = _financaBusiness.ObterTransacaoPorId(id);
        if (readTransacaoDto is null) 
            return NotFound(readTransacaoDto);
        return Ok(readTransacaoDto);
    }

    [HttpGet, Route("listar-transacoes")]
    public IActionResult ListarTransacoes()
    {
        List<ReadTransacaoDto> readTransacaoDtos = _financaBusiness.ListarTransacoes();
        if (readTransacaoDtos is null) 
            return NotFound();
        return Ok(readTransacaoDtos);
    }

    [HttpPut, Route("atualizar-transacao")]
    public IActionResult AtualizarTransacao([FromQuery] long id, [FromBody] UpdateTransacaoDto transacaoDto)
    {
        Result result = _financaBusiness.AtualizarTransacao(id, transacaoDto);
        if (result.IsFailed)
            return NotFound();
        return NoContent();
    }

    [HttpDelete, Route("remover-transacao")]
    public IActionResult RemoverTransacao([FromQuery] long id)
    {
        Result result = _financaBusiness.RemoverTransacao(id);
        if (result.IsFailed)
            return NotFound();
        return NoContent();
    }
}