using MediatR;
using MyFinances.Application.Authentication.Common.Responses;
using MyFinances.Application.Persistence.Authentication;
using MyFinances.Domain.Exception;

namespace MyFinances.Application.Authentication.Queries.Login;

public class LoginQueryHandler : IRequestHandler<LoginQuery, LoginUsuarioResponse>
{
    private readonly IJwtTokenGenarator _jwtTokenGenarator;
    private readonly IUsuarioRepository _usuarioRepository;

    public LoginQueryHandler(IJwtTokenGenarator jwtTokenGenarator, IUsuarioRepository usuarioRepository)
    {
        _jwtTokenGenarator = jwtTokenGenarator;
        _usuarioRepository = usuarioRepository;
    }

    public async Task<LoginUsuarioResponse> Handle(LoginQuery query, CancellationToken cancellationToken)
    {
        var usuario = await  _usuarioRepository.ObterPorEmailAsync(query.Email);

        if (usuario is null)
            throw new MyFinancesException(nameof(query.Email), MyFinancesExceptionType.BAD_REQUEST, "E-mail inválido.");

        if (!BCrypt.Net.BCrypt.Verify(query.Senha, usuario.SenhaHash))
            throw new MyFinancesException("Senha inválida.", MyFinancesExceptionType.UNAUTHORIZED);

        return _jwtTokenGenarator.GerarToken(usuario);
    }
}