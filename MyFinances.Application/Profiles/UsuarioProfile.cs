using AutoMapper;
using MyFinances.Application.Authentication.Commands.Cadastro;
using MyFinances.Application.Authentication.Common.Responses;
using MyFinances.Domain.Models;

namespace MyFinances.Application.Profiles;

public class UsuarioProfile : Profile
{
    public UsuarioProfile()
    {
        CreateMap<CadastroCommand, Usuario>();
        CreateMap<Usuario, RegistroUsuarioResponse>();
    }
}