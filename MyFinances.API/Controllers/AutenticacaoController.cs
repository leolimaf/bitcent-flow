using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyFinances.Application.Services.Authentication.Commands;
using MyFinances.Application.Services.Authentication.Common.Requests;
using MyFinances.Application.Services.Authentication.Common.Responses;
using MyFinances.Application.Services.Authentication.Queries;
using MyFinances.Application.Services.Interfaces;
using MyFinances.Domain.Exception;

namespace MyFinances.API.Controllers;

[ApiController]
[Authorize(Policy = "Bearer")]
[ApiVersion("1.0")]
[Route("v{version:apiVersion}/autenticacao")]
[Produces("application/json")]
public class AutenticacaoController : ControllerBase
{
    private IAutenticacaoCommandService _autenticacaoCommandService;
    private IAutenticacaoQueryService _autenticacaoQueryService;

    public AutenticacaoController(IAutenticacaoCommandService autenticacaoCommandService, IAutenticacaoQueryService autenticacaoQueryService)
    {
        _autenticacaoCommandService = autenticacaoCommandService;
        _autenticacaoQueryService = autenticacaoQueryService;
    }

    [HttpPost, Route("cadastrar"), AllowAnonymous]
    public async Task<IActionResult> Cadastrar([FromBody, Required] RegistroUsuarioRequest usuarioRequest)
    {
        try
        {
            RegistroUsuarioResponse registroUsuarioResponse = await _autenticacaoCommandService.CadastrarUsuario(usuarioRequest);
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
    public async Task<IActionResult> ObterPorId([Required] string id)
    {
        try
        {
            return Ok(await _autenticacaoQueryService.ObterUsuarioPorId(id));
        }
        catch (MyFinancesException e)
        {
            return StatusCode((int) e.ErrorType, e.ToErrorObject());
        }
    }

    [HttpPost, Route("logar"), AllowAnonymous]
    public async Task<IActionResult> Logar([FromBody, Required] LoginUsuarioRequest usuarioDto)
    {
        try
        {
            return Ok(await _autenticacaoQueryService.LogarUsuario(usuarioDto));
        }
        catch (MyFinancesException e)
        {
            return StatusCode((int) e.ErrorType, e.ToErrorObject());
        }
    }
    
    [HttpPost, Route("atualizar-token")]
    public async Task<IActionResult> AtualizarToken([FromBody, Required] AtualizacaoTokenRequest atualizacaoTokenRequest)
    {
        try
        {
            return Ok(await _autenticacaoCommandService.AtualizarToken(atualizacaoTokenRequest));
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
            await _autenticacaoCommandService.RevogarToken(User.Identity.Name);
            return NoContent();
        }
        catch (MyFinancesException e)
        {
            return StatusCode((int) e.ErrorType, e.ToErrorObject());
        }
    }
}