using Microsoft.EntityFrameworkCore.Infrastructure;
using MyFinances.Auth.Services.Interfaces;
using MyFinances.Domain.DTOs.Usuario;

namespace MyFinances.Test.Auth.Services;

public class UsuarioServiceTest : IClassFixture<TestFixture>
{
    private readonly IUsuarioService _usuarioService;

    public UsuarioServiceTest(TestFixture fixture)
    {
        _usuarioService = fixture.DbContext.GetService<IUsuarioService>();
    }

    [Fact(DisplayName = "Cadastrar Usuário")]
    public async Task TestarCadastrarUsuario()
    {
        // ARRANGE
        var createUsuarioDto = new CreateUsuarioDTO
        {
            Nome = "Usuario Três",
            Email = "usuario3@example.com",
            Senha = "123@abc"
        };
        
        // ACT
        var readUsuarioDto = await _usuarioService.CadastrarUsuario(createUsuarioDto);
        
        // ASSERT
        Assert.IsType<ReadUsuarioDTO>(readUsuarioDto);
    }
}