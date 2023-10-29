using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MyFinances.Domain.DTOs.Token;
using MyFinances.Domain.DTOs.Usuario;
using MyFinances.Domain.Models;
using MyFinances.Auth.Data;
using MyFinances.Auth.Services.Interfaces;

namespace MyFinances.Auth.Services;

public class UsuarioService : IUsuarioService
{
    private readonly IMapper _mapper;
    private readonly TokenService _tokenService;
    private readonly AppDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UsuarioService(IMapper mapper, TokenService tokenService, AppDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _mapper = mapper;
        _tokenService = tokenService;
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<ReadUsuarioDTO> CadastrarUsuario(CreateUsuarioDTO usuarioDto)
    {
        if (await _context.Usuarios.AnyAsync(x => x.Nome == usuarioDto.Nome))
            return new ReadUsuarioDTO{Message = "Nome de usuário já cadastrado."};

        if (await _context.Usuarios.AnyAsync(x => x.Email == usuarioDto.Email))
            return new ReadUsuarioDTO{Message = "E-mail já cadastrado."};
        
        Usuario usuario = _mapper.Map<Usuario>(usuarioDto);
        
        usuario.SenhaHash = BCrypt.Net.BCrypt.HashPassword(usuarioDto.Senha);
        
        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();

        return _mapper.Map<ReadUsuarioDTO>(usuario);
    }

    public async Task<ReadUsuarioDTO> ObterUsuarioPorId(string id)
    {
        var usuario = await _context.Usuarios.FirstOrDefaultAsync(x => x.Id.ToString() == id);
        
        return _mapper.Map<ReadUsuarioDTO>(usuario);
    }

    public async Task<TokenDTO?> LogarUsuario(CredenciaisDTO credenciaisDto)
    {
        var usuario = await  _context.Usuarios.FirstOrDefaultAsync(u => u.Email == credenciaisDto.Email);

        if (usuario is null || !BCrypt.Net.BCrypt.Verify(credenciaisDto.Senha, usuario.SenhaHash))
            return new TokenDTO{Message = "Usuário e / ou senha inválido(s)."};

        return _tokenService.GerarToken(usuario);
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
    
    public string ObterMeuEmail()
    {
        var usuario = string.Empty;
        // var roles = new List<string>();
        if (_httpContextAccessor.HttpContext is not null)
        {
            usuario = _httpContextAccessor.HttpContext.User.Identity?.Name;
            // var roleClaims = _httpContextAccessor.HttpContext.User.FindAll(ClaimTypes.Role);
            // roles = roleClaims.Select(c => c.Value).ToList();
        }
        return usuario;

    }
}