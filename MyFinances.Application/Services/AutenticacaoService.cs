using Mapster;
using MyFinances.Application.DTOs.Token;
using MyFinances.Application.DTOs.Usuario;
using MyFinances.Application.Persistence.Authentication;
using MyFinances.Application.Services.Interfaces;
using MyFinances.Domain.Exception;
using MyFinances.Domain.Models;

namespace MyFinances.Application.Services;

public class AutenticacaoService : IAutenticacaoService
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IJwtTokenGenarator _jwtTokenGenarator;

    public AutenticacaoService(IUsuarioRepository usuarioRepository, IJwtTokenGenarator jwtTokenGenarator)
    {
        _usuarioRepository = usuarioRepository;
        _jwtTokenGenarator = jwtTokenGenarator;
    }

    public async Task<ReadUsuarioDTO> Cadastrar(CreateUsuarioDTO usuarioDto)
    {
        if (await _usuarioRepository.IsCadastradoAsync(usuarioDto.Email))
            throw new MyFinancesException(nameof(usuarioDto.Email), MyFinancesExceptionType.CONFLICT, "E-mail já cadastrado.");

        Usuario? usuario = usuarioDto.Adapt<Usuario>();
        
        usuario.SenhaHash = BCrypt.Net.BCrypt.HashPassword(usuarioDto.Senha);
        
        await _usuarioRepository.CadastrarAsync(usuario);
        await _usuarioRepository.SalvarAlteracoesAsync();

        return usuario.Adapt<ReadUsuarioDTO>();
    }

    public async Task<ReadUsuarioDTO> ObterPorId(string id)
    {
        var usuario = await _usuarioRepository.ObterPorIdAsync(id);

        if (usuario is null)
            throw new MyFinancesException(nameof(id), MyFinancesExceptionType.NOT_FOUND, "Usuário não encontrado.");
        
        return usuario.Adapt<ReadUsuarioDTO>();
    }

    public async Task<ReadLoginUsuarioDTO> Logar(LoginUsuarioDTO usuarioDto)
    {
        var usuario = await  _usuarioRepository.ObterPorEmailAsync(usuarioDto.Email);

        if (usuario is null)
            throw new MyFinancesException(nameof(usuarioDto.Email), MyFinancesExceptionType.BAD_REQUEST, "E-mail inválido.");

        if (!BCrypt.Net.BCrypt.Verify(usuarioDto.Senha, usuario.SenhaHash))
            throw new MyFinancesException("Senha inválida.", MyFinancesExceptionType.UNAUTHORIZED);

        return _jwtTokenGenarator.GerarToken(usuario);
    }

    public async Task<ReadLoginUsuarioDTO> AtualizarToken(TokenDTO tokenDto)
    {
        var principal = _jwtTokenGenarator.GetPrincipalFromExpiredToken(tokenDto.AccessToken);
        var username = principal.Identity?.Name;

        var usuario = await _usuarioRepository.ObterPorEmailAsync(username);

        if (usuario is null || usuario.Token != tokenDto.RefreshToken || usuario.ValidadeToken <= DateTime.Now)
            throw new MyFinancesException(nameof(tokenDto.RefreshToken), MyFinancesExceptionType.UNAUTHORIZED, "Refresh Token inválido.");

        var accessToken = _jwtTokenGenarator.GerarAccessToken(principal.Claims);
        var refreshToken = _jwtTokenGenarator.GerarRefreshToken();

        usuario.Token = refreshToken;

        await _usuarioRepository.SalvarAlteracoesAsync();

        return _jwtTokenGenarator.RetornarTokenAtualizado(accessToken, refreshToken);
    }

    public async Task<bool> Deslogar(string email)
    {
        var usuario = await _usuarioRepository.ObterPorEmailAsync(email);
        
        if (usuario is null)
            throw new MyFinancesException(nameof(email), MyFinancesExceptionType.NOT_FOUND);
        
        usuario.Token = null;
        await _usuarioRepository.SalvarAlteracoesAsync();
        
        return true;
    }
}