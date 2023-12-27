using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFinances.Application.Authentication.Common.Responses;
using MyFinances.Application.Common.Interfaces;
using MyFinances.Application.Data;
using MyFinances.Domain.Exception;

namespace MyFinances.Application.Authentication.Queries.Login;

public class LoginQueryHandler : IRequestHandler<LoginQuery, LoginUsuarioResponse>
{
    private readonly IJwtTokenGenarator _jwtTokenGenarator;
    private readonly AppDbContext _context;

    public LoginQueryHandler(AppDbContext context, IJwtTokenGenarator jwtTokenGenarator)
    {
        _context = context;
        _jwtTokenGenarator = jwtTokenGenarator;
    }

    public async Task<LoginUsuarioResponse> Handle(LoginQuery query, CancellationToken cancellationToken)
    {
        var usuario = await  _context.Usuarios.FirstOrDefaultAsync(u => u.Email == query.Email);

        if (usuario is null)
            throw new MyFinancesException(nameof(query.Email), MyFinancesExceptionType.BAD_REQUEST, "E-mail inválido.");

        if (!BCrypt.Net.BCrypt.Verify(query.Senha, usuario.SenhaHash))
            throw new MyFinancesException("Senha inválida.", MyFinancesExceptionType.UNAUTHORIZED);

        return _jwtTokenGenarator.GerarToken(usuario);
    }
}