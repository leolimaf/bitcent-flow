using MediatR;

namespace MyFinances.Application.Authentication.Commands.Logoff;

public record LogoffCommand(string NomeDeUsuario) : IRequest<bool>;