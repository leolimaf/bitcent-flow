using Mapster;
using BitcentFlow.Application.DTOs.Usuario.Requests;
using BitcentFlow.Domain.Models;

namespace BitcentFlow.Tests.Mappers;

public class Mapper
{
    public static void ConfigureMapster()
    {
        TypeAdapterConfig<Usuario, RegistrationRequest>.NewConfig()
            .Map(dest => dest.ConfirmacaoDeSenhaSenha, src => src.Senha);
    }
}