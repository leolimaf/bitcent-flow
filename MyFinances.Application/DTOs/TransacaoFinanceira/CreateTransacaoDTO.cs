namespace MyFinances.Application.DTOs.TransacaoFinanceira;

public record CreateTransacaoDTO(
    string Descricao,
    DateTime Data,
    decimal Valor,
    TipoTransacaoDTO Tipo
);