using AutoMapper;
using MinhasFinancas.API.DTOs.Usuario;
using MinhasFinancas.API.Models;

namespace MinhasFinancas.API.Profiles;

public class UsuarioProfile : Profile
{
    public UsuarioProfile()
    {
        CreateMap<CreateUsuarioDTO, Usuario>();
        CreateMap<Usuario, ReadUsuarioDTO>()
            .ForMember(usuarioDto => usuarioDto.TransacaoFinanceiras, 
                opt => opt.MapFrom(usuario => usuario.TransacaoFinanceiras));
        CreateMap<UpdateUsuarioDTO, Usuario>();
    }
}