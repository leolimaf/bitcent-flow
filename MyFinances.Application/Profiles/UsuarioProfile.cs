using AutoMapper;
using MyFinances.Domain.Authentication.Requests;
using MyFinances.Domain.Authentication.Responses;
using MyFinances.Domain.Models;

namespace MyFinances.Application.Profiles;

public class UsuarioProfile : Profile
{
    public UsuarioProfile()
    {
        CreateMap<RegistroUsuarioRequest, Usuario>()
            .ForMember(u => u.SenhaHash, x => x.MapFrom(s => s.Senha));
        CreateMap<Usuario, RegistroUsuarioRequest>()
            .ForMember(u => u.Senha, x => x.MapFrom(s => s.SenhaHash));

        CreateMap<Usuario, RegistroUsuarioResponse>();
    }
}