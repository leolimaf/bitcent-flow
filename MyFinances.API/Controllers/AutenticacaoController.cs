using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyFinances.Application.DTOs.Token;
using MyFinances.Application.DTOs.Usuario;
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
        catch (MyFinancesException e)
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
        catch (MyFinancesException e)
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
        catch (MyFinancesException e)
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
            await _autenticacaoService.Deslogar(User.Identity.Name);
            return NoContent();
        }
        catch (MyFinancesException e)
        {
            return StatusCode((int) e.ErrorType, e.ToErrorObject());
        }
    }
}