using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFinances.Application.Data;
using MyFinances.Domain.Exception;

namespace MyFinances.Application.Authentication.Commands.Logoff;

public class LogoffCommandHandler : IRequestHandler<LogoffCommand, bool>
{
    private readonly AppDbContext _context;

    public LogoffCommandHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(LogoffCommand command, CancellationToken cancellationToken)
    {
        var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == command.NomeDeUsuario);
        
        if (usuario is null)
            throw new MyFinancesException(nameof(command.NomeDeUsuario), MyFinancesExceptionType.NOT_FOUND);
        
        usuario.Token = null;
        await _context.SaveChangesAsync(cancellationToken);
        
        return true;
    }
}