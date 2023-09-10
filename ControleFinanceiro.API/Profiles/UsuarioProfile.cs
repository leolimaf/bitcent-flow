using AutoMapper;
using ControleFinanceiro.API.DTOs.Usuario;
using ControleFinanceiro.API.Models;

namespace ControleFinanceiro.API.Profiles;

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