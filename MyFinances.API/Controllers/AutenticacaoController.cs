using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyFinances.Application.Services.Interfaces;
using MyFinances.Domain.Authentication.Requests;
using MyFinances.Domain.Authentication.Responses;
using MyFinances.Domain.Exception;

namespace MyFinances.API.Controllers;

[ApiController]
[Authorize(Policy = "Bearer")]
[ApiVersion("1.0")]
[Route("v{version:apiVersion}/autenticacao")]
[Produces("application/json")]
public class AutenticacaoController : ControllerBase
{
    private IAutenticacaoService _autenticacaoService;

    public AutenticacaoController(IAutenticacaoService autenticacaoService)
    {
        _autenticacaoService = autenticacaoService;
    }

    [HttpPost, Route("cadastrar"), AllowAnonymous]
    public async Task<IActionResult> Cadastrar([FromBody, Required] RegistroUsuarioRequest usuarioRequest)
    {
        try
        {
            RegistroUsuarioResponse registroUsuarioResponse = await _autenticacaoService.CadastrarUsuario(usuarioRequest);
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
            return Ok(await _autenticacaoService.ObterUsuarioPorId(id));
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
            return Ok(await _autenticacaoService.LogarUsuario(usuarioDto));
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
            return Ok(await _autenticacaoService.LogarUsuario(atualizacaoTokenRequest));
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
            await _autenticacaoService.RevogarToken(User.Identity.Name);
            return NoContent();
        }
        catch (MyFinancesException e)
        {
            return StatusCode((int) e.ErrorType, e.ToErrorObject());
        }
    }
}