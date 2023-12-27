using MediatR;
using MyFinances.Application.Authentication.Common.Responses;

namespace MyFinances.Application.Authentication.Commands.AtualizacaoToken;

public record AtualizacaoTokenCommand(
    string AccessToken,
    string RefreshToken
) : IRequest<LoginUsuarioResponse>;