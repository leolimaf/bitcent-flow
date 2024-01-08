using AutoMapper;
using MyFinances.Application.DTOs.TransacaoFinanceira;
using MyFinances.Domain.Models;

namespace MyFinances.Application.Profiles;

public class TransacaoFinanceiraProfile : Profile
{
    public TransacaoFinanceiraProfile()
    {
        CreateMap<CreateTransacaoDTO, TransacaoFinanceira>();
        CreateMap<TransacaoFinanceira, CreateTransacaoDTO>();
        
        CreateMap<TransacaoFinanceira, ReadTransacaoDTO>();
        CreateMap<UpdateTransacaoDTO, TransacaoFinanceira>();
    }
}