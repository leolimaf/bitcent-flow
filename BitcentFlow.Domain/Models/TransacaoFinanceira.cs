﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BitcentFlow.Domain.Models;

[Table("TRANSACAO_FINANCEIRA")]
public class TransacaoFinanceira
{
    [Key]
    [Required]
    [Column("ID")]
    public Guid Id { get; set; }
    
    [Required]
    [Column("DESCRICAO")]
    public string? Descricao { get; set; }
    
    [Required]
    [Column("DATA")]
    public DateTime Data { get; set; }
    
    [Required]
    [Column("VALOR")]
    [RegularExpression(@"^\d+(\.\d{1,2})?$")]
    public decimal Valor { get; set; }
    
    [Required]
    [Column("TIPO")]
    public TipoTransacao Tipo { get; set; }

    [Required] 
    [Column("USUARIO")]
    public string? Usuario { get; set; }
}

public enum TipoTransacao
{
    RECEITA,
    DESPESA
}