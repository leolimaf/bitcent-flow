using AutoMapper;
using MinhasFinancas.Auth.DTOs;
using MinhasFinancas.Auth.Models;

namespace MinhasFinancas.Auth.Profiles;

public class UsuarioProfile : Profile
{
    public UsuarioProfile()
    {
        CreateMap<CreateUsuarioDTO, Usuario>();
        CreateMap<Usuario, ReadUsuarioDTO>();
    }
}