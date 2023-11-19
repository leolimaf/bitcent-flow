using System.Security.Claims;
using AutoMapper;
using FluentResults;
using Microsoft.AspNetCore.JsonPatch;
using MyFinances.Domain.DTOs.TransacaoFinanceira;
using MyFinances.Domain.Models;
using MyFinances.API.Data;
using MyFinances.API.Services.Interfaces;
using MyFinances.Useful.Exception;
using Sieve.Models;
using Sieve.Services;

namespace MyFinances.API.Services;

public class TransacaoFinanceiraService : ITransacaoFinanceiraService
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly SieveProcessor _sieveProcessor;
    private readonly Guid _idUsuarioAutenticado;


    public TransacaoFinanceiraService(AppDbContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor, SieveProcessor sieveProcessor)
    {
        _context = context;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
        _sieveProcessor = sieveProcessor;
        _idUsuarioAutenticado = ObterIdDoUsuarioAutenticado();
    }

    public ReadTransacaoDTO AdicionarTransacao(CreateTransacaoDTO transacaoDto)
    {
        TransacaoFinanceira transacao = _mapper.Map<TransacaoFinanceira>(transacaoDto);
        transacao.IdUsuario = _idUsuarioAutenticado;
        _context.TransacoesFinanceiras.Add(transacao);
        _context.SaveChanges();
        return _mapper.Map<ReadTransacaoDTO>(transacao);
    }

    public ReadTransacaoDTO ObterTransacaoPorId(Guid id)
    {
        var transacao = _context.TransacoesFinanceiras.Find(id);

        if (transacao is null)
            throw new MyFinancesException(nameof(id), MyFinancesExceptionType.NOT_FOUND, "Transação financeira não encontrada.");
            
        return _mapper.Map<ReadTransacaoDTO>(transacao);
    }

    public List<ReadTransacaoDTO> ListarTransacoes(SieveModel model)
    {
        var transacoes = _context.TransacoesFinanceiras
            .Where(x => x.IdUsuario == _idUsuarioAutenticado);

        var readTransacaoDto = _mapper.Map<List<ReadTransacaoDTO>>(transacoes).AsQueryable();
        
        readTransacaoDto = _sieveProcessor.Apply(model, readTransacaoDto);
        
        return readTransacaoDto.ToList();
    }

    public Result AtualizarTransacao(Guid id, UpdateTransacaoDTO transacaoDto)
    {
        var transacao = _context.TransacoesFinanceiras.Find(id);
        
        if (transacao is null)
            throw new MyFinancesException(nameof(id), MyFinancesExceptionType.NOT_FOUND, "Transação financeira não encontrada.");
        
        transacao.IdUsuario = _idUsuarioAutenticado;

        _mapper.Map(transacaoDto, transacao);
        _context.SaveChanges();
        return Result.Ok();
    }

    public Result AtualizarTransacaoParcialmente(Guid id, JsonPatchDocument transacaoDto)
    {
        var transacao = _context.TransacoesFinanceiras.Find(id);
        
        var idUsuario = _idUsuarioAutenticado;
        
        if (transacao is null)
            throw new MyFinancesException("Transação financeira não encontrada.");
        
        transacao.IdUsuario = idUsuario;
        
        transacaoDto.ApplyTo(transacao);
        _context.SaveChanges();
        return Result.Ok();
    }

    public Result RemoverTransacao(Guid id)
    {
        var transacao = _context.TransacoesFinanceiras.Find(id);

        if (transacao is null)
            throw new MyFinancesException(nameof(id), MyFinancesExceptionType.NOT_FOUND, "Transação financeira não encontrada.");
        
        transacao.IdUsuario = _idUsuarioAutenticado;

        _context.Remove(transacao);
        _context.SaveChanges();
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