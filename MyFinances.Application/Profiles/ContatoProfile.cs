using AutoMapper;
using MyFinances.Application.DTOs.Contato;
using MyFinances.Domain.Models;

namespace MyFinances.Application.Profiles;

public class ContatoProfile : Profile
{
    public ContatoProfile()
    {
        CreateMap<ContatoDTO, Contato>();
        CreateMap<Contato, ContatoDTO>();
    }
}