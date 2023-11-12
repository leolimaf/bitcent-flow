using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MyFinances.Domain.DTOs.Token;
using MyFinances.Domain.DTOs.Usuario;
using MyFinances.Domain.Models;
using MyFinances.Auth.Data;
using MyFinances.Auth.Services.Interfaces;
using MyFinances.Useful.Exception;

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
            throw new MyFinancesException(nameof(usuarioDto.Nome), MyFinancesExceptionType.DATABASE_EXECUTION, $"The {nameof(usuarioDto.Nome)} field is already registered.");

        if (await _context.Usuarios.AnyAsync(x => x.Email == usuarioDto.Email))
            throw new MyFinancesException(nameof(usuarioDto.Email), MyFinancesExceptionType.DATABASE_EXECUTION, $"The {nameof(usuarioDto.Email)} field is already registered.");

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
    
    public async Task<TokenDTO?> LogarUsuario(TokenValueDTO tokenValueDto)
    {
        try
        {
            var principal = _tokenService.GetPrincipalFromExpiredToken(tokenValueDto.AccessToken);
            var username = principal.Identity?.Name;
        
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == username);
        
            if (usuario is null || usuario.Token != tokenValueDto.RefreshToken || usuario.ValidadeToken <= DateTime.Now)
                return null;
        
            var accessToken = _tokenService.GerarAccessToken(principal.Claims);
            var refreshToken = _tokenService.GerarRefreshToken();
        
            usuario.Token = refreshToken;

            var user = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == usuario.Email);
        
            if (user is null)
                return null;
        
            await _context.SaveChangesAsync();
        
            return _tokenService.RetornarTokenAtualizado(accessToken, refreshToken);
        }
        catch (SecurityTokenExpiredException e)
        {
            return new TokenDTO
            {
                Message = e.Message
            };
        }
    }

    public async Task<bool> RevogarToken(string nomeDeUsuario)
    {
        var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == nomeDeUsuario);
        
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