using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MinhasFinancas.Auth.Data;
using MinhasFinancas.Auth.DTOs;
using MinhasFinancas.Auth.Models;
using MinhasFinancas.Auth.Services.Interfaces;

namespace MinhasFinancas.Auth.Services;

public class UsuarioService : IUsuarioService
{
    private IMapper _mapper;
    
    private TokenService _tokenService;
    private AppDbContext _context;

    public UsuarioService(IMapper mapper, TokenService tokenService, AppDbContext context)
    {
        _mapper = mapper;
        _tokenService = tokenService;
        _context = context;
    }

    public async Task<ReadUsuarioDTO> CadastrarUsuario(CreateUsuarioDTO usuarioDto)
    {
        Usuario usuario = _mapper.Map<Usuario>(usuarioDto);
        
        usuario.Senha = ComputarHash(usuario.Senha, SHA256.Create());
        
        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();

        return _mapper.Map<ReadUsuarioDTO>(usuario);
    }

    public async Task<ReadUsuarioDTO> ObterUsuarioPorId(long id)
    {
        var usuario = await _context.Usuarios.FirstOrDefaultAsync(x => x.Id == id);
        
        return _mapper.Map<ReadUsuarioDTO>(usuario);
    }

    public async Task<TokenDto?> LogarUsuario(LoginUsuarioDTO credenciais)
    {
        var senha = ComputarHash(credenciais.Senha, SHA256.Create());
        var usuario = await  _context.Usuarios.FirstOrDefaultAsync(u => (u.Nome == credenciais.Nome && u.Senha == senha) 
                                                                        || u.Email == credenciais.Email && u.Senha == senha);

        return usuario is not null ? 
            _tokenService.GenerateToken(usuario) :
            null;
    }
    
    public static string ComputarHash(string entrada, SHA256 algoritmo)
    {
        byte[] inputBytes = Encoding.UTF8.GetBytes(entrada);
        byte[] hashedBytes = algoritmo.ComputeHash(inputBytes);
        return BitConverter.ToString(hashedBytes);
    }
}