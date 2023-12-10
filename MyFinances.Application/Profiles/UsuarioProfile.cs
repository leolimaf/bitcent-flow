using AutoMapper;
using MyFinances.Domain.Authentication.Requests;
using MyFinances.Domain.Authentication.Responses;
using MyFinances.Domain.Models;

namespace MyFinances.Application.Profiles;

public class UsuarioProfile : Profile
{
    public UsuarioProfile()
    {
        CreateMap<RegistroUsuarioRequest, Usuario>();
        CreateMap<Usuario, RegistroUsuarioResponse>();
    }
}