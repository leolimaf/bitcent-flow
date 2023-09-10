using AutoMapper;
using ControleFinanceiro.API.DTOs.Usuario;
using ControleFinanceiro.API.Models;

namespace ControleFinanceiro.API.Profiles;

public class UsuarioProfile : Profile
{
    public UsuarioProfile()
    {
        CreateMap<CreateUsuarioDTO, Usuario>();
        CreateMap<Usuario, ReadUsuarioDTO>();
        CreateMap<UpdateUsuarioDTO, Usuario>();
    }
}