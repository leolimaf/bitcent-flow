using MediatR;
using MyFinances.Application.Authentication.Common.Responses;

namespace MyFinances.Application.Authentication.Queries.Identificacao;

public record IdentificacaoQuery(string Id) : IRequest<RegistroUsuarioResponse>;