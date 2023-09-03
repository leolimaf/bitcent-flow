using AutoMapper;
using ControleFinanceiro.API.DTOs.TransacaoFinanceira;
using ControleFinanceiro.API.Models;

namespace ControleFinanceiro.API.Profiles;

public class TransacaoFinanceiraProfile : Profile
{
    public TransacaoFinanceiraProfile()
    {
        CreateMap<CreateTransacaoDto, TransacaoFinanceira>();
        CreateMap<TransacaoFinanceira, ReadTransacaoDto>();
        CreateMap<UpdateTransacaoDto, TransacaoFinanceira>();
    }
}