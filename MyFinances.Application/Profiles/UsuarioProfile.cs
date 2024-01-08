using AutoMapper;
using MyFinances.Application.DTOs.Usuario;
using MyFinances.Domain.Models;

namespace MyFinances.Application.Profiles;

public class UsuarioProfile : Profile
{
    public UsuarioProfile()
    {
        CreateMap<CreateUsuarioDTO, Usuario>();
        CreateMap<Usuario, ReadUsuarioDTO>();
    }
}