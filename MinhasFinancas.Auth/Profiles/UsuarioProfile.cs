using AutoMapper;
using MinhasFinancas.Domain.DTOs.Usuario;
using MinhasFinancas.Domain.Models;

namespace MinhasFinancas.Auth.Profiles;

public class UsuarioProfile : Profile
{
    public UsuarioProfile()
    {
        CreateMap<CreateUsuarioDTO, Usuario>();
        CreateMap<Usuario, ReadUsuarioDTO>();
    }
}