using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MyFinances.Application.Authentication.Commands.AtualizacaoToken;
using MyFinances.Application.Authentication.Commands.Cadastro;
using MyFinances.Application.Authentication.Common.Responses;
using MyFinances.Application.Common.Interfaces;
using MyFinances.Application.Data;
using MyFinances.Application.Services.Interfaces;
using MyFinances.Domain.Exception;
using MyFinances.Domain.Models;

namespace MyFinances.Application.Services.Authentication.Commands;

public class AutenticacaoCommandService : IAutenticacaoCommandService
{
    private readonly IMapper _mapper;
    private readonly IJwtTokenGenarator _jwtTokenGenarator;
    private readonly AppDbContext _context;

    public AutenticacaoCommandService(IMapper mapper, IJwtTokenGenarator jwtTokenGenarator, AppDbContext context)
    {
        _mapper = mapper;
        _jwtTokenGenarator = jwtTokenGenarator;
        _context = context;
    }

    public async Task<RegistroUsuarioResponse> CadastrarUsuario(CadastroCommand usuarioRequest)
    {
        if (await _context.Usuarios.AnyAsync(x => x.Email == usuarioRequest.Email))
            throw new MyFinancesException(nameof(usuarioRequest.Email), MyFinancesExceptionType.CONFLICT, "E-mail já cadastrado.");

        Usuario usuario = _mapper.Map<Usuario>(usuarioRequest);
        
        usuario.SenhaHash = BCrypt.Net.BCrypt.HashPassword(usuarioRequest.Senha);
        
        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();

        return _mapper.Map<RegistroUsuarioResponse>(usuario);
    }
    
    public async Task<LoginUsuarioResponse?> AtualizarToken(AtualizacaoTokenCommand atualizacaoTokenCommand)
    {
        var principal = _jwtTokenGenarator.GetPrincipalFromExpiredToken(atualizacaoTokenCommand.AccessToken);
        var username = principal.Identity?.Name;

        var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == username);

        if (usuario is null || usuario.Token != atualizacaoTokenCommand.RefreshToken || usuario.ValidadeToken <= DateTime.Now)
            throw new MyFinancesException(nameof(atualizacaoTokenCommand.RefreshToken), MyFinancesExceptionType.UNAUTHORIZED, "Refresh Token inválido.");

        var accessToken = _jwtTokenGenarator.GerarAccessToken(principal.Claims);
        var refreshToken = _jwtTokenGenarator.GerarRefreshToken();

        usuario.Token = refreshToken;

        await _context.SaveChangesAsync();

        return _jwtTokenGenarator.RetornarTokenAtualizado(accessToken, refreshToken);
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