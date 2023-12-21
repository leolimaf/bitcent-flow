using MyFinances.Application.Services.Authentication.Common.Requests;
using MyFinances.Application.Services.Authentication.Common.Responses;

namespace MyFinances.Application.Services.Authentication.Queries;

public interface IAutenticacaoQueryService
{
    Task<RegistroUsuarioResponse> ObterUsuarioPorId(string id);
    Task<LoginUsuarioResponse?> LogarUsuario(LoginUsuarioRequest loginUsuarioRequest);
}