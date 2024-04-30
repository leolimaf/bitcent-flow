using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BitcentFlow.Application.DTOs.Token;
using BitcentFlow.Application.DTOs.Usuario;
using BitcentFlow.Application.Services.Interfaces;
using BitcentFlow.Domain.Exception;

namespace BitcentFlow.API.Controllers;

[ApiController]
[Authorize(Policy = "Bearer")]
[ApiVersion("1.0")]
[Route("v{version:apiVersion}/autenticacao")]
[Produces("application/json")]
public class AutenticacaoController : ControllerBase
{

    private readonly IAutenticacaoService _autenticacaoService;

    public AutenticacaoController(IAutenticacaoService autenticacaoService)
    {
        _autenticacaoService = autenticacaoService;
    }

    [HttpPost, Route("cadastrar"), AllowAnonymous]
    public async Task<IActionResult> Cadastrar([FromBody, Required] CreateUsuarioDTO usuarioRequest)
    {
        try
        {
            ReadUsuarioDTO readUsuarioDto = await _autenticacaoService.Cadastrar(usuarioRequest);
            return CreatedAtAction(nameof(ObterPorId), new {version = HttpContext.GetRequestedApiVersion()!.ToString(),readUsuarioDto.Id}, readUsuarioDto);
        }
        catch (BitcentFlowException e)
        {
            return StatusCode((int) e.ErrorType, e.ToErrorObject());
        }
    }
    
    [Authorize(Roles = "Administrator")]
    [HttpGet, Route("obter-por-id")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public async Task<IActionResult> ObterPorId([Required, FromQuery] string id)
    {
        try
        {
            return Ok(await _autenticacaoService.ObterPorId(id));
        }
        catch (BitcentFlowException e)
        {
            return StatusCode((int) e.ErrorType, e.ToErrorObject());
        }
    }

    [HttpPost, Route("logar"), AllowAnonymous]
    public async Task<IActionResult> Logar([FromBody, Required] LoginUsuarioDTO usuarioDto)
    {
        try
        {
            return Ok(await _autenticacaoService.Logar(usuarioDto));
        }
        catch (BitcentFlowException e)
        {
            return StatusCode((int) e.ErrorType, e.ToErrorObject());
        }
    }
    
    [HttpPost, Route("atualizar-token")]
    public async Task<IActionResult> AtualizarToken([FromBody, Required] TokenDTO tokenDto)
    {
        try
        {
            return Ok(await _autenticacaoService.AtualizarToken(tokenDto));
        }
        catch (BitcentFlowException e)
        {
            return StatusCode((int) e.ErrorType, e.ToErrorObject());
        }
    }
    
    [HttpPost, Route("deslogar")]
    public async Task<IActionResult> Deslogar()
    {
        try
        {
            await _autenticacaoService.Deslogar(User.Identity.Name);
            return NoContent();
        }
        catch (BitcentFlowException e)
        {
            return StatusCode((int) e.ErrorType, e.ToErrorObject());
        }
    }
}