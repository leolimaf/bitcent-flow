using MediatR;
using MyFinances.Application.Authentication.Common.Responses;

namespace MyFinances.Application.Authentication.Queries.Login;

public record LoginQuery(
    string Email,
    string Senha
)  : IRequest<LoginUsuarioResponse>;