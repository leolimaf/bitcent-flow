using AutoMapper;
using ControleFinanceiro.API.DTOs.TransacaoFinanceira;
using ControleFinanceiro.API.Models;

namespace ControleFinanceiro.API.Profiles;

public class TransacaoFinanceiraProfile : Profile
{
    public TransacaoFinanceiraProfile()
    {
        CreateMap<CreateTransacaoDTO, TransacaoFinanceira>();
        CreateMap<TransacaoFinanceira, ReadTransacaoDTO>();
        CreateMap<UpdateTransacaoDTO, TransacaoFinanceira>();
    }
}