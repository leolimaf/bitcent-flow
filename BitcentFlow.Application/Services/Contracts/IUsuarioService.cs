using BitcentFlow.Application.DTOs.Usuario.Requests;
using BitcentFlow.Application.DTOs.Usuario.Responses;

namespace BitcentFlow.Application.Services.Contracts;

public interface IUsuarioService
{
    Task<RegistrationResponse> RegistrarUsuarioAsync(RegistrationRequest registrationRequest);
    Task<LoginResponse> LogarUsuarioAsync(LoginRequest loginRequest);
}