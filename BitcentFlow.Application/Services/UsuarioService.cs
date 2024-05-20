using BitcentFlow.Application.DTOs.Usuario;
using BitcentFlow.Application.DTOs.Usuario.Requests;
using BitcentFlow.Application.DTOs.Usuario.Responses;
using BitcentFlow.Application.Persistence.Contracts;
using BitcentFlow.Application.Services.Contracts;
using BitcentFlow.Domain.Models;
using Mapster;

namespace BitcentFlow.Application.Services;

public class UsuarioService(IUsuarioRepository usuarioRepository , IJwtGenarator jwtGenarator) : IUsuarioService
{
    public async Task<RegistrationResponse> RegistrarUsuarioAsync(RegistrationRequest registrationRequest)
    {
        var usuario = await usuarioRepository.ObterPorEmailAsync(registrationRequest.Email);

        if (usuario is not null)
            return new RegistrationResponse(false, "Usuário já cadastrado.");

        usuario = registrationRequest.Adapt<Usuario>();
        usuario.SenhaHash = BCrypt.Net.BCrypt.HashPassword(registrationRequest.Senha);
        
        var sucessoAoRegistrar = await usuarioRepository.RegistrarAsync(usuario);

        return sucessoAoRegistrar > 0
            ? new RegistrationResponse(true, "Usuário cadastrado com sucesso.")
            : new RegistrationResponse(false, "Falha ao cadastrar usuário.");
    }

    public async Task<LoginResponse> LogarUsuarioAsync(LoginRequest loginRequest)
    {
        var usuario = await usuarioRepository.ObterPorEmailAsync(loginRequest.Email);

        if (usuario is null || !BCrypt.Net.BCrypt.Verify(loginRequest.Senha, usuario.SenhaHash))
            return new LoginResponse(false, "Credenciais Inválidas.");

        var token = await jwtGenarator.GerarToken(usuario);

        return new LoginResponse(true, "Autenticação realizada com sucesso.", token);
    }

    public async Task<LoginResponse> AtualizarTokenUsuarioAsync(TokenDTO tokenDto)
    {
        var principal = jwtGenarator.GetPrincipalFromExpiredToken(tokenDto.AccessToken);
        var id = Guid.Parse(principal.Identity?.Name ?? "");

        var usuario = await usuarioRepository.ObterPorIdAsync(id);

        if (usuario is null || usuario.Token != tokenDto.RefreshToken || usuario.ValidadeToken <= DateTime.Now)
            return new LoginResponse(false, "Token inválido.");

        var token = await jwtGenarator.GerarToken(usuario, principal.Claims);

        return new LoginResponse(true, "Atualização do token realizada com sucesso.", token);
    }
}