using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyFinances.Domain.DTOs.Token;
using MyFinances.Domain.DTOs.Usuario;
using MyFinances.Auth.Services.Interfaces;
using MyFinances.Useful.Exception;

namespace MyFinances.Auth.Controllers;

[ApiController]
[Authorize(Policy = "Bearer")]
[Route("usuario")]
[Produces("application/json")]
public class UsuarioController : ControllerBase
{
    private IUsuarioService _usuarioService;

    public UsuarioController(IUsuarioService usuarioService)
    {
        _usuarioService = usuarioService;
    }

    [HttpPost, Route("cadastrar"), AllowAnonymous]
    public async Task<IActionResult> Cadastrar([FromBody, Required] CreateUsuarioDTO usuarioDto)
    {
        try
        {
            ReadUsuarioDTO readUsuarioDto = await _usuarioService.CadastrarUsuario(usuarioDto);
            return CreatedAtAction(nameof(ObterPorId), readUsuarioDto.Id, readUsuarioDto);
        }
        catch (MyFinancesException e)
        {
            return StatusCode((int) e.ErrorType, e.ToErrorObject());
        }
    }
    
    [Authorize(Roles = "Administrator")]
    [HttpGet, Route("obter-por-id")]
    public async Task<IActionResult> ObterPorId([Required] string id)
    {
        try
        {
            return Ok(await _usuarioService.ObterUsuarioPorId(id));
        }
        catch (MyFinancesException e)
        {
            return StatusCode((int) e.ErrorType, e.ToErrorObject());
        }
    }

    [HttpPost, Route("logar"), AllowAnonymous]
    public async Task<IActionResult> Logar([FromBody, Required] CredenciaisDTO usuarioDto)
    {
        try
        {
            return Ok(await _usuarioService.LogarUsuario(usuarioDto));
        }
        catch (MyFinancesException e)
        {
            return StatusCode((int) e.ErrorType, e.ToErrorObject());
        }
    }
    
    [HttpPost, Route("atualizar-token")]
    public async Task<IActionResult> AtualizarToken([FromBody, Required] TokenValueDTO tokenValueDto)
    {
        try
        {
            return Ok(await _usuarioService.LogarUsuario(tokenValueDto));
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
            await _usuarioService.RevogarToken(User.Identity.Name);
            return NoContent();
        }
        catch (MyFinancesException e)
        {
            return StatusCode((int) e.ErrorType, e.ToErrorObject());
        }
    }
}