using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFinances.Application.Authentication.Common.Responses;
using MyFinances.Application.Common.Interfaces;
using MyFinances.Application.Data;
using MyFinances.Domain.Exception;

namespace MyFinances.Application.Authentication.Commands.AtualizacaoToken;

public class AtualizacaoTokenCommandHandler  : IRequestHandler<AtualizacaoTokenCommand, LoginUsuarioResponse>
{
    private readonly IJwtTokenGenarator _jwtTokenGenarator;
    private readonly AppDbContext _context;

    public AtualizacaoTokenCommandHandler(IJwtTokenGenarator jwtTokenGenarator, AppDbContext context)
    {
        _jwtTokenGenarator = jwtTokenGenarator;
        _context = context;
    }

    public async Task<LoginUsuarioResponse> Handle(AtualizacaoTokenCommand command, CancellationToken cancellationToken)
    {
        var principal = _jwtTokenGenarator.GetPrincipalFromExpiredToken(command.AccessToken);
        var username = principal.Identity?.Name;

        var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == username);

        if (usuario is null || usuario.Token != command.RefreshToken || usuario.ValidadeToken <= DateTime.Now)
            throw new MyFinancesException(nameof(command.RefreshToken), MyFinancesExceptionType.UNAUTHORIZED, "Refresh Token inválido.");

        var accessToken = _jwtTokenGenarator.GerarAccessToken(principal.Claims);
        var refreshToken = _jwtTokenGenarator.GerarRefreshToken();

        usuario.Token = refreshToken;

        await _context.SaveChangesAsync(cancellationToken);

        return _jwtTokenGenarator.RetornarTokenAtualizado(accessToken, refreshToken);
    }
}