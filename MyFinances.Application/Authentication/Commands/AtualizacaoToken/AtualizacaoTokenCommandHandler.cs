using MediatR;
using MyFinances.Application.Authentication.Common.Responses;
using MyFinances.Application.Persistence.Authentication;
using MyFinances.Domain.Exception;

namespace MyFinances.Application.Authentication.Commands.AtualizacaoToken;

public class AtualizacaoTokenCommandHandler  : IRequestHandler<AtualizacaoTokenCommand, LoginUsuarioResponse>
{
    private readonly IJwtTokenGenarator _jwtTokenGenarator;
    private readonly IUsuarioRepository _usuarioRepository;

    public AtualizacaoTokenCommandHandler(IJwtTokenGenarator jwtTokenGenarator, IUsuarioRepository usuarioRepository)
    {
        _jwtTokenGenarator = jwtTokenGenarator;
        _usuarioRepository = usuarioRepository;
    }

    public async Task<LoginUsuarioResponse> Handle(AtualizacaoTokenCommand command, CancellationToken cancellationToken)
    {
        var principal = _jwtTokenGenarator.GetPrincipalFromExpiredToken(command.AccessToken);
        var username = principal.Identity?.Name;

        var usuario = await _usuarioRepository.ObterPorEmailAsync(username);

        if (usuario is null || usuario.Token != command.RefreshToken || usuario.ValidadeToken <= DateTime.Now)
            throw new MyFinancesException(nameof(command.RefreshToken), MyFinancesExceptionType.UNAUTHORIZED, "Refresh Token inválido.");

        var accessToken = _jwtTokenGenarator.GerarAccessToken(principal.Claims);
        var refreshToken = _jwtTokenGenarator.GerarRefreshToken();

        usuario.Token = refreshToken;

        await _usuarioRepository.SalvarAlteracoesAsync(cancellationToken);

        return _jwtTokenGenarator.RetornarTokenAtualizado(accessToken, refreshToken);
    }
}