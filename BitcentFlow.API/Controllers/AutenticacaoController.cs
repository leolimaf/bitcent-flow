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

    /// <summary>Cadastra um usuário</summary>
    /// <remarks>Realiza o cadastro do usuário que poderá gerenciar suas transações financeiras.</remarks>
    /// <response code="201">Requisição realizada com sucesso</response>
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
    
    /// <summary>Obtem um usuário</summary>
    /// <remarks>Obtem um usuário a partir do seu id.</remarks>
    /// <response code="200">Requisição realizada com sucesso</response>
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

    /// <summary>Autentica um usuário</summary>
    /// <remarks>Autentica o usuário por meio de suas credenciais.</remarks>
    /// <response code="200">Requisição realizada com sucesso</response>
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
    
    /// <summary>Atualiza o token do usuário</summary>
    /// <remarks>Atualiza o access e refresh token do usuário autenticado através dos atuais.</remarks>
    /// <response code="200">Requisição realizada com sucesso</response>
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
    
    /// <summary>Desloga o usuário</summary>
    /// <remarks>Invalida o token do usuário autenticado.</remarks>
    /// <response code="204">Requisição realizada com sucesso</response>
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