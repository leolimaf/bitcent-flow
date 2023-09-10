using AutoMapper;
using MinhasFinancas.API.DTOs.TransacaoFinanceira;
using MinhasFinancas.API.Models;

namespace MinhasFinancas.API.Profiles;

public class TransacaoFinanceiraProfile : Profile
{
    public TransacaoFinanceiraProfile()
    {
        CreateMap<CreateTransacaoDTO, TransacaoFinanceira>();
        CreateMap<TransacaoFinanceira, ReadTransacaoDTO>();
        CreateMap<UpdateTransacaoDTO, TransacaoFinanceira>();
    }
}