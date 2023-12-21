using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MyFinances.Application.Authentication.Common.Responses;
using MyFinances.Application.Authentication.Queries.Login;
using MyFinances.Application.Common.Interfaces;
using MyFinances.Application.Data;
using MyFinances.Domain.Exception;

namespace MyFinances.Application.Services.Authentication.Queries;

public class AutenticacaoQueryService : IAutenticacaoQueryService
{
    private readonly IMapper _mapper;
    private readonly IJwtTokenGenarator _jwtTokenGenarator;
    private readonly AppDbContext _context;

    public AutenticacaoQueryService(IMapper mapper, IJwtTokenGenarator jwtTokenGenarator, AppDbContext context)
    {
        _mapper = mapper;
        _jwtTokenGenarator = jwtTokenGenarator;
        _context = context;
    }

    public async Task<RegistroUsuarioResponse> ObterUsuarioPorId(string id)
    {
        var usuario = await _context.Usuarios.FirstOrDefaultAsync(x => x.Id.ToString() == id);

        if (usuario is null)
            throw new MyFinancesException(nameof(id), MyFinancesExceptionType.NOT_FOUND, "Usuário não encontrado.");
        
        return _mapper.Map<RegistroUsuarioResponse>(usuario);
    }

    public async Task<LoginUsuarioResponse?> LogarUsuario(LoginQuery loginQuery)
    {
        var usuario = await  _context.Usuarios.FirstOrDefaultAsync(u => u.Email == loginQuery.Email);

        if (usuario is null)
            throw new MyFinancesException(nameof(loginQuery.Email), MyFinancesExceptionType.BAD_REQUEST, "E-mail inválido.");

        if (!BCrypt.Net.BCrypt.Verify(loginQuery.Senha, usuario.SenhaHash))
            throw new MyFinancesException("Senha inválida.", MyFinancesExceptionType.UNAUTHORIZED);

        return _jwtTokenGenarator.GerarToken(usuario);
    }
}