using System.Security.Claims;
using AutoMapper;
using FluentResults;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using MyFinances.Application.DTOs.TransacaoFinanceira;
using MyFinances.Application.Persistence;
using MyFinances.Application.Persistence.TransacaoFinanceira;
using MyFinances.Application.Services.Interfaces;
using MyFinances.Domain.Exception;
using MyFinances.Domain.Models;
using Sieve.Models;
using Sieve.Services;

namespace MyFinances.Application.Services;

public class TransacaoFinanceiraService : ITransacaoFinanceiraService
{
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly SieveProcessor _sieveProcessor;
    private readonly Guid _idUsuarioAutenticado;
    private readonly ITransacaoFinanceiraRepository _transacaoRepository;


    public TransacaoFinanceiraService(ITransacaoFinanceiraRepository transacaoRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor, SieveProcessor sieveProcessor)
    {
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
        _sieveProcessor = sieveProcessor;
        _transacaoRepository = transacaoRepository;
        _idUsuarioAutenticado = ObterIdDoUsuarioAutenticado();
    }

    public ReadTransacaoDTO AdicionarTransacao(CreateTransacaoDTO transacaoDto)
    {
        TransacaoFinanceira transacao = _mapper.Map<TransacaoFinanceira>(transacaoDto);
        transacao.IdUsuario = _idUsuarioAutenticado;
        _transacaoRepository.Adicionar(transacao);
        _transacaoRepository.SalvarAlteracoes();
        return _mapper.Map<ReadTransacaoDTO>(transacao);
    }

    public ReadTransacaoDTO ObterTransacaoPorId(Guid id)
    {
        var transacao = _transacaoRepository.ObterPorId(id);

        if (transacao is null)
            throw new MyFinancesException(nameof(id), MyFinancesExceptionType.NOT_FOUND, "Transação financeira não encontrada.");
            
        return _mapper.Map<ReadTransacaoDTO>(transacao);
    }

    public List<ReadTransacaoDTO> ListarTransacoes(SieveModel model)
    {
        var transacoes = _transacaoRepository.Listar();

        var readTransacaoDto = _mapper.Map<List<ReadTransacaoDTO>>(transacoes).AsQueryable();
        
        readTransacaoDto = _sieveProcessor.Apply(model, readTransacaoDto);
        
        return readTransacaoDto.ToList();
    }

    public Result AtualizarTransacao(Guid id, UpdateTransacaoDTO transacaoDto)
    {
        var transacao = _transacaoRepository.ObterPorId(id);
        
        if (transacao is null)
            throw new MyFinancesException(nameof(id), MyFinancesExceptionType.NOT_FOUND, "Transação financeira não encontrada.");
        
        transacao.IdUsuario = _idUsuarioAutenticado;

        _mapper.Map(transacaoDto, transacao);
        _transacaoRepository.SalvarAlteracoes();
        return Result.Ok();
    }

    public Result AtualizarTransacaoParcialmente(Guid id, JsonPatchDocument transacaoDto)
    {
        var transacao = _transacaoRepository.ObterPorId(id);
        
        var idUsuario = _idUsuarioAutenticado;
        
        if (transacao is null)
            throw new MyFinancesException("Transação financeira não encontrada.");
        
        transacao.IdUsuario = idUsuario;
        
        transacaoDto.ApplyTo(transacao);
        _transacaoRepository.SalvarAlteracoes();
        return Result.Ok();
    }

    public Result RemoverTransacao(Guid id)
    {
        var transacao = _transacaoRepository.ObterPorId(id);

        if (transacao is null)
            throw new MyFinancesException(nameof(id), MyFinancesExceptionType.NOT_FOUND, "Transação financeira não encontrada.");
        
        transacao.IdUsuario = _idUsuarioAutenticado;

        _transacaoRepository.Remover(transacao);
        _transacaoRepository.SalvarAlteracoes();
        return Result.Ok();
    }
    
    private Guid ObterIdDoUsuarioAutenticado()
    {
        var idUsuario = _httpContextAccessor.HttpContext?.User.Claims
            .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        if (idUsuario is null)
            throw new MyFinancesException(nameof(idUsuario), MyFinancesExceptionType.UNAUTHORIZED);
        
        return new Guid(idUsuario);
    }
}