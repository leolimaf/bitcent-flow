using AutoMapper;
using MyFinances.Domain.DTOs.Usuario;
using MyFinances.Domain.Models;

namespace MyFinances.Auth.Profiles;

public class UsuarioProfile : Profile
{
    public UsuarioProfile()
    {
        CreateMap<CreateUsuarioDTO, Usuario>();
        CreateMap<Usuario, ReadUsuarioDTO>();
    }
}