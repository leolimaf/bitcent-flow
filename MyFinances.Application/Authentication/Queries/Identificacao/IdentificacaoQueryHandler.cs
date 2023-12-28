using AutoMapper;
using MediatR;
using MyFinances.Application.Authentication.Common.Responses;
using MyFinances.Application.Persistence.Authentication;
using MyFinances.Domain.Exception;

namespace MyFinances.Application.Authentication.Queries.Identificacao;


public class IdentificacaoQueryHandler : IRequestHandler<IdentificacaoQuery, RegistroUsuarioResponse>
{
    
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IMapper _mapper;

    public IdentificacaoQueryHandler(IUsuarioRepository usuarioRepository, IMapper mapper)
    {
        _usuarioRepository = usuarioRepository;
        _mapper = mapper;
    }

    public async Task<RegistroUsuarioResponse> Handle(IdentificacaoQuery query, CancellationToken cancellationToken)
    {
        var usuario = await _usuarioRepository.ObterPorIdAsync(query.Id);

        if (usuario is null)
            throw new MyFinancesException(nameof(query.Id), MyFinancesExceptionType.NOT_FOUND, "Usuário não encontrado.");
        
        return _mapper.Map<RegistroUsuarioResponse>(usuario);
    }
}