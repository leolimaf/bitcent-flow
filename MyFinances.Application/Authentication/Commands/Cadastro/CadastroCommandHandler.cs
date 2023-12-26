using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFinances.Application.Authentication.Common.Responses;
using MyFinances.Application.Common.Interfaces;
using MyFinances.Application.Data;
using MyFinances.Domain.Exception;
using MyFinances.Domain.Models;

namespace MyFinances.Application.Authentication.Commands.Cadastro;

public class CadastroCommandHandler : IRequestHandler<CadastroCommand, RegistroUsuarioResponse>
{
    private readonly IMapper _mapper;
    private readonly IJwtTokenGenarator _jwtTokenGenarator;
    private readonly AppDbContext _context;

    public CadastroCommandHandler(IMapper mapper, IJwtTokenGenarator jwtTokenGenarator, AppDbContext context)
    {
        _mapper = mapper;
        _jwtTokenGenarator = jwtTokenGenarator;
        _context = context;
    }

    public async Task<RegistroUsuarioResponse> Handle(CadastroCommand command, CancellationToken cancellationToken)
    {
        if (await _context.Usuarios.AnyAsync(x => x.Email == command.Email, cancellationToken: cancellationToken))
            throw new MyFinancesException(nameof(command.Email), MyFinancesExceptionType.CONFLICT, "E-mail já cadastrado.");

        Usuario usuario = _mapper.Map<Usuario>(command);
        
        usuario.SenhaHash = BCrypt.Net.BCrypt.HashPassword(command.Senha);
        
        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<RegistroUsuarioResponse>(usuario);
    }
}