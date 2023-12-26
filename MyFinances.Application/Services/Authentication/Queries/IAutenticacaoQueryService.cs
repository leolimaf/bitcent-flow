using MyFinances.Application.Authentication.Common.Responses;
using MyFinances.Application.Authentication.Queries.Login;

namespace MyFinances.Application.Services.Authentication.Queries;

public interface IAutenticacaoQueryService
{
    Task<RegistroUsuarioResponse> ObterUsuarioPorId(string id);
    Task<LoginUsuarioResponse?> LogarUsuario(LoginQuery loginQuery);
}