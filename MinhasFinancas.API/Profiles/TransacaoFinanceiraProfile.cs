using AutoMapper;
using MinhasFinancas.Domain.DTOs.TransacaoFinanceira;
using MinhasFinancas.Domain.Models;

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