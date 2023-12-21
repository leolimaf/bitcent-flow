using MediatR;
using MyFinances.Application.Authentication.Common.Responses;

namespace MyFinances.Application.Authentication.Commands.Cadastro;

public record CadastroCommand(
    string Nome,
    string Email,
    string Senha
) : IRequest<RegistroUsuarioResponse>;