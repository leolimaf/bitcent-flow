using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MinhasFinancas.Auth.Data;
using MinhasFinancas.Auth.DTOs;
using MinhasFinancas.Auth.DTOs.Token;
using MinhasFinancas.Auth.DTOs.Usuario;
using MinhasFinancas.Auth.Models;
using MinhasFinancas.Auth.Services.Interfaces;

namespace MinhasFinancas.Auth.Services;

public class UsuarioService : IUsuarioService
{
    private IMapper _mapper;
    private TokenService _tokenService;
    private AppDbContext _context;

    public UsuarioService(IMapper mapper, TokenService tokenService, AppDbContext context)
    {
        _mapper = mapper;
        _tokenService = tokenService;
        _context = context;
    }

    public async Task<ReadUsuarioDTO> CadastrarUsuario(CreateUsuarioDTO usuarioDto)
    {
        Usuario usuario = _mapper.Map<Usuario>(usuarioDto);
        
        usuario.Senha = ComputarHash(usuario.Senha, SHA256.Create());
        
        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();

        return _mapper.Map<ReadUsuarioDTO>(usuario);
    }

    public async Task<ReadUsuarioDTO> ObterUsuarioPorId(long id)
    {
        var usuario = await _context.Usuarios.FirstOrDefaultAsync(x => x.Id == id);
        
        return _mapper.Map<ReadUsuarioDTO>(usuario);
    }

    public async Task<TokenDTO?> LogarUsuario(CredenciaisDTO credenciaisDto)
    {
        var senha = ComputarHash(credenciaisDto.Senha, SHA256.Create());
        var usuario = await  _context.Usuarios.FirstOrDefaultAsync(u => (u.Nome == credenciaisDto.Nome && u.Senha == senha) 
                                                                        || u.Email == credenciaisDto.Email && u.Senha == senha);

        return usuario is not null ? 
            _tokenService.GerarToken(usuario) :
            null;
    }
    
    public async Task<TokenDTO> LogarUsuario(TokenValueDTO tokenValueDto)
    {
        var principal = _tokenService.GetPrincipalFromExpiredToken(tokenValueDto.AccessToken);
        var username = principal.Identity?.Name;
        
        var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Nome == username);
        
        if (usuario is null || usuario.Token != tokenValueDto.RefreshToken || usuario.ValidadeToken <= DateTime.Now)
            return null!;
        
        var accessToken = _tokenService.GerarAccessToken(principal.Claims);
        var refreshToken = _tokenService.GerarRefreshToken();
        
        usuario.Token = refreshToken;

        var user = await _context.Usuarios.FirstOrDefaultAsync(u => u.Nome == usuario.Nome || u.Email == usuario.Email);
        
        if (user is null)
            return null;
        
        await _context.SaveChangesAsync();
        
        return _tokenService.RetornarTokenAtualizado(accessToken, refreshToken);
    }

    public async Task<bool> RevogarToken(string nomeDeUsuario)
    {
        var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Nome == nomeDeUsuario);
        
        if (usuario is null)
            return false;
        
        usuario.Token = null;
        await _context.SaveChangesAsync();
        
        return true;
    }

    private static string ComputarHash(string entrada, SHA256 algoritmo)
    {
        byte[] inputBytes = Encoding.UTF8.GetBytes(entrada);
        byte[] hashedBytes = algoritmo.ComputeHash(inputBytes);
        return BitConverter.ToString(hashedBytes);
    }
}