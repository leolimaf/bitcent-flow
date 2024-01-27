using Mapster;
using MyFinances.Application.DTOs.Usuario;
using MyFinances.Domain.Models;

namespace MyFinances.Tests.Mappers;

public class Mapper
{
    public static void ConfigureMapster()
    {
        TypeAdapterConfig<Usuario, CreateUsuarioDTO>.NewConfig()
            .Map(dest => dest.ConfirmacaoDeSenha, src => src.Senha);
    }
}