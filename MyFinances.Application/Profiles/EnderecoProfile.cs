using AutoMapper;
using MyFinances.Application.DTOs.Endereco;
using MyFinances.Domain.Models;

namespace MyFinances.Application.Profiles;

public class EnderecoProfile : Profile
{
    public EnderecoProfile()
    {
        CreateMap<EnderecoDTO, Endereco>();
        CreateMap<Endereco, EnderecoDTO>();
    }
}