using Mapster;
using BitcentFlow.Application.DTOs.Usuario;
using BitcentFlow.Domain.Models;

namespace BitcentFlow.Tests.Mappers;

public class Mapper
{
    public static void ConfigureMapster()
    {
        TypeAdapterConfig<Usuario, CreateUsuarioDTO>.NewConfig()
            .Map(dest => dest.ConfirmacaoDeSenha, src => src.Senha);
    }
}