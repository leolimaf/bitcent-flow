using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MyFinances.Domain.DTOs.Token;
using MyFinances.Domain.DTOs.Usuario;
using MyFinances.Domain.Models;
using MyFinances.Auth.Data;
using MyFinances.Auth.Services.Interfaces;
using MyFinances.Useful.Date;
using MyFinances.Useful.Exception;

namespace MyFinances.Auth.Services;

public class UsuarioService : IUsuarioService
{
    private readonly IMapper _mapper;
    private readonly TokenService _tokenService;
    private readonly AppDbContext _context;

    public UsuarioService(IMapper mapper, TokenService tokenService, AppDbContext context)
    {
        _mapper = mapper;
        _tokenService = tokenService;
        _context = context;
    }

    public async Task<ReadUsuarioDTO> CadastrarUsuario(CreateUsuarioDTO usuarioDto)
    {
        if (await _context.Usuarios.AnyAsync(x => x.Email == usuarioDto.Email))
            throw new MyFinancesException(nameof(usuarioDto.Email), MyFinancesExceptionType.CONFLICT, "E-mail já cadastrado.");

        Usuario usuario = _mapper.Map<Usuario>(usuarioDto);
        
        usuario.SenhaHash = BCrypt.Net.BCrypt.HashPassword(usuarioDto.Senha);
        
        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();

        return _mapper.Map<ReadUsuarioDTO>(usuario);
    }

    public async Task<ReadUsuarioDTO> ObterUsuarioPorId(string id)
    {
        var usuario = await _context.Usuarios.FirstOrDefaultAsync(x => x.Id.ToString() == id);

        if (usuario is null)
            throw new MyFinancesException(nameof(id), MyFinancesExceptionType.NOT_FOUND, "Usuário não encontrado.");
        
        return _mapper.Map<ReadUsuarioDTO>(usuario);
    }

    public async Task<TokenDTO?> LogarUsuario(CredenciaisDTO credenciaisDto)
    {
        var usuario = await  _context.Usuarios.FirstOrDefaultAsync(u => u.Email == credenciaisDto.Email);

        if (usuario is null)
            throw new MyFinancesException(nameof(credenciaisDto.Email), MyFinancesExceptionType.BAD_REQUEST, "E-mail inválido.");

        if (!BCrypt.Net.BCrypt.Verify(credenciaisDto.Senha, usuario.SenhaHash))
            throw new MyFinancesException("Senha inválida.", MyFinancesExceptionType.UNAUTHORIZED);

        return _tokenService.GerarToken(usuario);
    }
    
    public async Task<TokenDTO?> LogarUsuario(TokenValueDTO tokenValueDto)
    {
        var principal = _tokenService.GetPrincipalFromExpiredToken(tokenValueDto.AccessToken);
        var username = principal.Identity?.Name;

        var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == username);

        if (usuario is null || usuario.Token != tokenValueDto.RefreshToken || usuario.ValidadeToken <= DataInterna.ObterHorarioDeBrasilia())
            throw new MyFinancesException(nameof(tokenValueDto.RefreshToken), MyFinancesExceptionType.UNAUTHORIZED, "Refresh Token inválido.");

        var accessToken = _tokenService.GerarAccessToken(principal.Claims);
        var refreshToken = _tokenService.GerarRefreshToken();

        usuario.Token = refreshToken;

        await _context.SaveChangesAsync();

        return _tokenService.RetornarTokenAtualizado(accessToken, refreshToken);
    }

    public async Task<bool> RevogarToken(string nomeDeUsuario)
    {
        var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == nomeDeUsuario);
        
        if (usuario is null)
            throw new MyFinancesException(nameof(nomeDeUsuario), MyFinancesExceptionType.NOT_FOUND);
        
        usuario.Token = null;
        await _context.SaveChangesAsync();
        
        return true;
    }
}