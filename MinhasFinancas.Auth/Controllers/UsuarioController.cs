using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MinhasFinancas.Auth.DTOs.Token;
using MinhasFinancas.Auth.DTOs.Usuario;
using MinhasFinancas.Auth.Services.Interfaces;

namespace MinhasFinancas.Auth.Controllers;

[ApiController]
[Route("usuario")]
[Produces("application/json")]
public class UsuarioController : ControllerBase
{
    private IUsuarioService _usuarioService;

    public UsuarioController(IUsuarioService usuarioService)
    {
        _usuarioService = usuarioService;
    }

    [HttpPost, Route("cadastrar")]
    public async Task<IActionResult> Cadastrar([FromBody] CreateUsuarioDTO usuarioDto)
    {
        ReadUsuarioDTO readUsuarioDto = await _usuarioService.CadastrarUsuario(usuarioDto);

        if (!string.IsNullOrWhiteSpace(readUsuarioDto.Flag))
            return BadRequest(readUsuarioDto.Flag);
        
        return CreatedAtAction(nameof(ObterPorId), readUsuarioDto.Id, readUsuarioDto);
    }
    
    [HttpGet, Route("obter-por-id")]
    [Authorize(Policy = "Bearer")]
    public async Task<IActionResult> ObterPorId(long id)
    {
        ReadUsuarioDTO readUsuarioDto = await _usuarioService.ObterUsuarioPorId(id);

        if (readUsuarioDto is null)
            return NotFound(readUsuarioDto);

        if (readUsuarioDto.Nome != User.Identity.Name)
            return Forbid();
        
        return Ok(readUsuarioDto);
    }

    [HttpPost, Route("logar")]
    public async Task<IActionResult> Logar([FromBody] CredenciaisDTO usuarioDto)
    {
        var token = await _usuarioService.LogarUsuario(usuarioDto);

        if (token is not null && !string.IsNullOrWhiteSpace(token.Flag))
            return Unauthorized(token.Flag);

        return token is not null ? 
            Ok(token) : 
            Unauthorized();
    }
    
    [HttpPost, Route("atualizar-token")]
    [Authorize(Policy = "Bearer")]
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
    [Authorize(Policy = "Bearer")]
    public async Task<IActionResult> Deslogar()
    {
        var result = await _usuarioService.RevogarToken(User.Identity.Name);

        if (!result)
            return BadRequest();
        
        return NoContent();
    }
}