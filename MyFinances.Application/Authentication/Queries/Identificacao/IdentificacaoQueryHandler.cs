using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyFinances.Application.Authentication.Common.Responses;
using MyFinances.Application.Data;
using MyFinances.Domain.Exception;

namespace MyFinances.Application.Authentication.Queries.Identificacao;


public class IdentificacaoQueryHandler : IRequestHandler<IdentificacaoQuery, RegistroUsuarioResponse>
{
    
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public IdentificacaoQueryHandler(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<RegistroUsuarioResponse> Handle(IdentificacaoQuery query, CancellationToken cancellationToken)
    {
        var usuario = await _context.Usuarios.FirstOrDefaultAsync(x => x.Id.ToString() == query.Id);

        if (usuario is null)
            throw new MyFinancesException(nameof(query.Id), MyFinancesExceptionType.NOT_FOUND, "Usuário não encontrado.");
        
        return _mapper.Map<RegistroUsuarioResponse>(usuario);
    }
}