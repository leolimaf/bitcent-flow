using Microsoft.AspNetCore.Mvc;
using MinhasFinancas.Auth.DTOs;
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
        return CreatedAtAction(nameof(ObterPorId), readUsuarioDto.Id, readUsuarioDto);
    }
    
    [HttpGet, Route("obter-por-id")]
    public async Task<IActionResult> ObterPorId(long id)
    {
        ReadUsuarioDTO readUsuarioDto = await _usuarioService.ObterUsuarioPorId(id);
        
        return readUsuarioDto is not null ? 
            Ok(readUsuarioDto) : 
            NotFound(readUsuarioDto);
    }

    [HttpPost, Route("logar")]
    public async Task<IActionResult> Logar([FromBody] LoginUsuarioDTO usuarioDto)
    {
        var token = await _usuarioService.LogarUsuario(usuarioDto);

        return token is not null ? 
            Ok(token) : 
            Unauthorized();
    }
}