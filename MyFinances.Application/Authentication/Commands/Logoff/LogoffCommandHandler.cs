using MediatR;
using MyFinances.Application.Persistence.Authentication;
using MyFinances.Domain.Exception;

namespace MyFinances.Application.Authentication.Commands.Logoff;

public class LogoffCommandHandler : IRequestHandler<LogoffCommand, bool>
{
    private readonly IUsuarioRepository _usuarioRepository;

    public LogoffCommandHandler(IUsuarioRepository usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
    }

    public async Task<bool> Handle(LogoffCommand command, CancellationToken cancellationToken)
    {
        var usuario = await _usuarioRepository.ObterPorEmailAsync(command.NomeDeUsuario);
        
        if (usuario is null)
            throw new MyFinancesException(nameof(command.NomeDeUsuario), MyFinancesExceptionType.NOT_FOUND);
        
        usuario.Token = null;
        await _usuarioRepository.SalvarAlteracoesAsync(cancellationToken);
        
        return true;
    }
}