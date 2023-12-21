using AutoMapper;
using MyFinances.Application.Services.Authentication.Common.Requests;
using MyFinances.Application.Services.Authentication.Common.Responses;
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