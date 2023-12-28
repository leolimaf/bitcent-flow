using AutoMapper;
using MediatR;
using MyFinances.Application.Authentication.Common.Responses;
using MyFinances.Application.Persistence.Authentication;
using MyFinances.Domain.Exception;
using MyFinances.Domain.Models;

namespace MyFinances.Application.Authentication.Commands.Cadastro;

public class CadastroCommandHandler : IRequestHandler<CadastroCommand, RegistroUsuarioResponse>
{
    private readonly IMapper _mapper;
    private readonly IUsuarioRepository _usuarioRepository;

    public CadastroCommandHandler(IMapper mapper, IUsuarioRepository usuarioRepository)
    {
        _mapper = mapper;
        _usuarioRepository = usuarioRepository;
    }

    public async Task<RegistroUsuarioResponse> Handle(CadastroCommand command, CancellationToken cancellationToken)
    {
        if (await _usuarioRepository.IsCadastradoAsync(command.Email, cancellationToken))
            throw new MyFinancesException(nameof(command.Email), MyFinancesExceptionType.CONFLICT, "E-mail já cadastrado.");

        Usuario usuario = _mapper.Map<Usuario>(command);
        
        usuario.SenhaHash = BCrypt.Net.BCrypt.HashPassword(command.Senha);
        
        await _usuarioRepository.CadastrarAsync(usuario);
        await _usuarioRepository.SalvarAlteracoesAsync(cancellationToken);

        return _mapper.Map<RegistroUsuarioResponse>(usuario);
    }
}