namespace BitcentFlow.Application.DTOs.TransacaoFinanceira;

public record UpdateTransacaoDTO(
    string Descricao,
    DateTime Data,
    decimal Valor,
    TipoTransacaoDTO Tipo
);