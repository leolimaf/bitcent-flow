using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyFinances.Auth.IdentityData;
using MyFinances.Domain.DTOs.Token;
using MyFinances.Domain.DTOs.Usuario;
using MyFinances.Auth.Services.Interfaces;

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

    [AllowAnonymous]
    [HttpPost, Route("cadastrar")]
    public async Task<IActionResult> Cadastrar([FromBody] CreateUsuarioDTO usuarioDto)
    {
        ReadUsuarioDTO readUsuarioDto = await _usuarioService.CadastrarUsuario(usuarioDto);

        if (!string.IsNullOrWhiteSpace(readUsuarioDto.Message))
            return BadRequest(readUsuarioDto.Message);
        
        return CreatedAtAction(nameof(ObterPorId), readUsuarioDto.Id, readUsuarioDto);
    }
    
    [Authorize(Policy = "Admin")]
    [RequiresClaim(ClaimTypes.Role, "Administrator")]
    [HttpGet, Route("obter-por-id")]
    public async Task<IActionResult> ObterPorId(string id)
    {
        ReadUsuarioDTO readUsuarioDto = await _usuarioService.ObterUsuarioPorId(id);

        if (readUsuarioDto is null)
            return NotFound(readUsuarioDto);
        
        return Ok(readUsuarioDto);
    }

    [AllowAnonymous]
    [HttpPost, Route("logar")]
    public async Task<IActionResult> Logar([FromBody] CredenciaisDTO usuarioDto)
    {
        var token = await _usuarioService.LogarUsuario(usuarioDto);

        if (token is not null && !string.IsNullOrWhiteSpace(token.Message))
            return Unauthorized(token.Message);

        return token is not null ? 
            Ok(token) : 
            Unauthorized();
    }
    
    [AllowAnonymous]
    [HttpPost, Route("atualizar-token")]
    public async Task<IActionResult> AtualizarToken([FromBody] TokenValueDTO tokenValueDto)
    {
        if (string.IsNullOrWhiteSpace(tokenValueDto.AccessToken) || string.IsNullOrWhiteSpace(tokenValueDto.RefreshToken))
            return BadRequest();

        var token = await _usuarioService.LogarUsuario(tokenValueDto);

        if (token is null)
            return Unauthorized();
        
        return Ok(token);
    }
    
    [HttpPost, Route("deslogar")]
    public async Task<IActionResult> Deslogar()
    {
        var result = await _usuarioService.RevogarToken(User.Identity.Name);

        if (!result)
            return BadRequest();
        
        return NoContent();
    }
}