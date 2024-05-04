namespace BitcentFlow.Application.DTOs.TransacaoFinanceira;

public record CreateTransacaoDTO(
    string Descricao,
    DateTime Data,
    decimal Valor,
    TipoTransacaoDTO Tipo,
    Guid IdUsuario
);