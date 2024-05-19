namespace BitcentFlow.Application.DTOs.Usuario.Responses;

public record LoginResponse(bool Flag, string Mensagem = null!, string Token = null!);