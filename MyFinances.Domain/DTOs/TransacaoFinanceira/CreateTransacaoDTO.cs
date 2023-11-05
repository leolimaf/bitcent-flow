﻿using System.ComponentModel.DataAnnotations;
using MyFinances.Domain.Models;

namespace MyFinances.Domain.DTOs.TransacaoFinanceira;

public class CreateTransacaoDTO
{
    [Required]
    public string Descricao { get; set; }
    
    [Required]
    public DateTime Data { get; set; }
    
    [Required]
    public double Valor { get; set; }
    
    [Required]
    public TipoTransacao Tipo { get; set; }
}