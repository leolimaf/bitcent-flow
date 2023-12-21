using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyFinances.Application.Authentication.Commands.AtualizacaoToken;
using MyFinances.Application.Authentication.Commands.Cadastro;
using MyFinances.Application.Authentication.Commands.Logoff;
using MyFinances.Application.Authentication.Common.Responses;
using MyFinances.Application.Authentication.Queries.Identificacao;
using MyFinances.Application.Authentication.Queries.Login;
using MyFinances.Domain.Exception;

namespace MyFinances.API.Controllers;

[ApiController]
[Authorize(Policy = "Bearer")]
[ApiVersion("1.0")]
[Route("v{version:apiVersion}/autenticacao")]
[Produces("application/json")]
public class AutenticacaoController : ControllerBase
{
    private readonly ISender _mediator;

    public AutenticacaoController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpPost, Route("cadastrar"), AllowAnonymous]
    public async Task<IActionResult> Cadastrar([FromBody, Required] CadastroCommand usuarioRequest)
    {
        try
        {
            RegistroUsuarioResponse registroUsuarioResponse = await _mediator.Send(usuarioRequest);
            return CreatedAtAction(nameof(ObterPorId), new {version = HttpContext.GetRequestedApiVersion()!.ToString(),registroUsuarioResponse.Id}, registroUsuarioResponse);
        }
        catch (MyFinancesException e)
        {
            return StatusCode((int) e.ErrorType, e.ToErrorObject());
        }
    }
    
    [Authorize(Roles = "Administrator")]
    [HttpGet, Route("obter-por-id")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public async Task<IActionResult> ObterPorId([Required] IdentificacaoQuery id)
    {
        try
        {
            return Ok(await _mediator.Send(id));
        }
        catch (MyFinancesException e)
        {
            return StatusCode((int) e.ErrorType, e.ToErrorObject());
        }
    }

    [HttpPost, Route("logar"), AllowAnonymous]
    public async Task<IActionResult> Logar([FromBody, Required] LoginQuery usuarioDto)
    {
        try
        {
            return Ok(await _mediator.Send(usuarioDto));
        }
        catch (MyFinancesException e)
        {
            return StatusCode((int) e.ErrorType, e.ToErrorObject());
        }
    }
    
    [HttpPost, Route("atualizar-token")]
    public async Task<IActionResult> AtualizarToken([FromBody, Required] AtualizacaoTokenCommand atualizacaoTokenCommand)
    {
        try
        {
            return Ok(await _mediator.Send(atualizacaoTokenCommand));
        }
        catch (MyFinancesException e)
        {
            return StatusCode((int) e.ErrorType, e.ToErrorObject());
        }
    }
    
    [HttpPost, Route("deslogar")]
    public async Task<IActionResult> Deslogar()
    {
        try
        {
            await _mediator.Send(new LogoffCommand(User.Identity.Name));
            return NoContent();
        }
        catch (MyFinancesException e)
        {
            return StatusCode((int) e.ErrorType, e.ToErrorObject());
        }
    }
}