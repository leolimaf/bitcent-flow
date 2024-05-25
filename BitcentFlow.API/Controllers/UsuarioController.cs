using BitcentFlow.Application.DTOs.Usuario;
using BitcentFlow.Application.DTOs.Usuario.Requests;
using BitcentFlow.Application.DTOs.Usuario.Responses;
using BitcentFlow.Application.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BitcentFlow.API.Controllers;

[ApiController]
[Authorize("Bearer")]
[ApiVersion("1.0")]
[Route("v{version:apiVersion}/usuarios")]
[Produces("application/json")]
public class UsuarioController(IUsuarioService usuarioService) : ControllerBase
{
    [HttpPost("registrar"), AllowAnonymous]
    public async Task<ActionResult<RegistrationResponse>> Registrar(RegistrationRequest registrationRequest)
    {
        var result = await usuarioService.RegistrarUsuarioAsync(registrationRequest);
        return result.Cadastrado ? Ok(result) : BadRequest(result);
    }

    
    [HttpPost("logar"), AllowAnonymous]
    public async Task<ActionResult<LoginResponse>> Logar(LoginRequest loginRequest)
    {
        var result = await usuarioService.LogarUsuarioAsync(loginRequest);
        return result.Autenticado ? Ok(result) : BadRequest(result);
    }
    
    [HttpPost("atualizar-token")]
    public async Task<ActionResult<LoginResponse>> AtualizarToken(TokenDTO tokenDto)
    {
        var result = await usuarioService.AtualizarTokenUsuarioAsync(tokenDto);
        return result.Autenticado ? Ok(result) : BadRequest(result);
    }
    
    [HttpPost("deslogar")]
    public async Task<IActionResult> Deslogar()
    {
        await usuarioService.DeslogarUsuarioAsync();
        return NoContent();
    }
}