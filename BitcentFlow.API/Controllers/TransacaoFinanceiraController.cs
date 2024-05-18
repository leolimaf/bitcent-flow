using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using BitcentFlow.Application.DTOs.TransacaoFinanceira;
using BitcentFlow.Application.Services.Interfaces;
using BitcentFlow.Domain.Exception;
using Microsoft.AspNetCore.Authorization;
using Sieve.Models;

namespace BitcentFlow.API.Controllers;

[ApiController]
[Authorize]
[ApiVersion("1.0")]
[Route("v{version:apiVersion}/transacoes-financeiras")]
[Produces("application/json")]
public class TransacaoFinanceiraController(ITransacaoFinanceiraService transacaoFinanceiraService) : ControllerBase
{
    /// <summary> Adiciona uma transação financeira</summary>
    /// <remarks>Realiza a entrada das receitas e despesas do usuário autenticado.</remarks>
    /// <response code="201">Requisição realizada com sucesso</response>
    [HttpPost, Route("adicionar")]
    [ProducesResponseType(201, Type = typeof(ReadTransacaoDTO))]
    [ProducesResponseType(400, Type = typeof(ErroDTO))]
    public async Task<IActionResult> AdicionarTransacaoFinanceira([FromBody, Required] CreateTransacaoDTO transacaoDto)
    {
        try
        {
            ReadTransacaoDTO readTransacaoDto = await transacaoFinanceiraService.AdicionarTransacao(transacaoDto);
            return CreatedAtAction(nameof(ObterTransacaoPorId), new {version = HttpContext.GetRequestedApiVersion()!.ToString(), readTransacaoDto.Id} , readTransacaoDto);
        }
        catch (BitcentFlowException e)
        {
            return StatusCode((int) e.ErrorType, e.ToErrorObject());
        }
    }
    
    /// <summary>Obtem uma transação financeira</summary>
    /// <remarks>A partir do identificador de uma transação financeira do usuário autenticado, é possível obte-lá.</remarks>
    /// <param name="id">Identificador da transação financeira</param>
    /// <response code="200">Requisição realizada com sucesso</response>
    /// <response code="404">A transação não foi encontrada</response>
    [HttpGet, Route("obter-por-id")]
    [ProducesResponseType(200, Type = typeof(ReadTransacaoDTO))]
    [ProducesResponseType(404, Type = typeof(ErroDTO))]
    public async Task<IActionResult> ObterTransacaoPorId([Required] Guid id)
    {
        try
        {
            ReadTransacaoDTO readTransacaoDto = await transacaoFinanceiraService.ObterTransacaoPorId(id);
            return Ok(readTransacaoDto);
        }
        catch (BitcentFlowException e)
        {
            return StatusCode((int) e.ErrorType, e.ToErrorObject());
        }
    }

    /// <summary>Lista todas as transações financeiras</summary>
    /// <remarks>
    /// Retorna todas as transações financeiras do usuário autenticado. Alguns filtros podem ser aplicados conforme os exemplos logo abaixo:
    /// 
    ///     Filters: descricao@=Conta de Luz
    ///     Sorts: valor
    ///     Page: 2
    ///     PageSize: 1
    /// 
    /// </remarks>
    /// <response code="200">Requisição realizada com sucesso</response>
    [HttpGet, Route("listar")]
    [ProducesResponseType(200, Type = typeof(List<ReadTransacaoDTO>))]
    public async Task<IActionResult> ListarTransacoes([FromQuery] SieveModel model)
    {
        try
        {
            List<ReadTransacaoDTO> readTransacaoDtos = await transacaoFinanceiraService.ListarTransacoes(model);
            return Ok(readTransacaoDtos);
        }
        catch (BitcentFlowException e)
        {
            return StatusCode((int) e.ErrorType, e.ToErrorObject());
        }
        
    }

    /// <summary>Atualiza uma transação financeira</summary>
    /// <remarks>A partir do identificador de uma transação financeira do usuário autenticado, é possível editá-la.</remarks>
    /// <param name="id">Identificador da transação financeira</param>
    /// <response code="204">Requisição realizada com sucesso</response>
    /// <response code="404">A transação não foi encontrada</response>
    [HttpPut, Route("atualizar")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404, Type = typeof(ErroDTO))]
    public async Task<IActionResult> AtualizarTransacao([FromQuery, Required] Guid id, [FromBody, Required] UpdateTransacaoDTO transacaoDto)
    {
        try
        {
            await transacaoFinanceiraService.AtualizarTransacao(id, transacaoDto);
            return NoContent();
        }
        catch (BitcentFlowException e)
        {
            return StatusCode((int) e.ErrorType, e.ToErrorObject());
        }
    }
    
    /// <summary>Atualiza uma transação financeira parcialmente</summary>
    /// <remarks>
    /// A partir do identificador de uma transação financeira do usuário autenticado, é possível atualiza-la  utilizando o verbo Patch do Http.
    ///
    /// Exemplo:
    /// 
    ///     [
    ///         {
    ///             "path": "descricao",
    ///             "op": "replace",
    ///             "value": "Conta de Água Atualizada"
    ///         }
    ///     ]
    /// 
    /// </remarks>
    /// <param name="id">Identificador da transação financeira</param>
    /// <response code="204">Requisição realizada com sucesso</response>
    /// <response code="404">A transação não foi encontrada</response>
    [HttpPatch, Route("atualizar-parcialmente")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404, Type = typeof(ErroDTO))]
    public async Task<IActionResult> AtualizarTransacaoParcialmente([FromQuery, Required] Guid id, [FromBody] JsonPatchDocument transacaoDto)
    {
        try
        {
            await transacaoFinanceiraService.AtualizarTransacaoParcialmente(id, transacaoDto);
            return NoContent();
        }
        catch (BitcentFlowException e)
        {
            return StatusCode((int) e.ErrorType, e.ToErrorObject());
        }
    }

    /// <summary>Remove uma transação financeira</summary>
    /// <remarks>A partir do identificador de uma transação financeira do usuário autenticado, é possível apagá-la.</remarks>
    /// <param name="id">Identificador da transação financeira</param>
    /// <response code="204">Requisição realizada com sucesso</response>
    /// <response code="404">A transação não foi encontrada</response>
    [HttpDelete, Route("remover")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404, Type = typeof(ErroDTO))]
    public async Task<IActionResult> RemoverTransacao([FromQuery, Required] Guid id)
    {
        try
        {
            await transacaoFinanceiraService.RemoverTransacao(id);
            return NoContent();
        }
        catch (BitcentFlowException e)
        {
            return StatusCode((int) e.ErrorType, e.ToErrorObject());
        }
    }
}