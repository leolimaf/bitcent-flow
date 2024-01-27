using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MyFinances.Domain.Models;

[Table("ENDERECO")]
public class Endereco
{
    [Key]
    [Required]
    [Column("ID")]
    public Guid Id { get; set; }

    [Column("RUA")]
    public string Rua { get; set; }
    
    [Column("NUMERO")]
    public string Numero { get; set; }
    
    [Column("COMPLEMENTO")]
    public string Complemento { get; set; }
    
    [Column("BAIRRO")]
    public string Bairro { get; set; }

    [Column("CIDADE")]
    public string Cidade { get; set; }
    
    [Column("ESTADO")]
    public string Estado { get; set; }
    
    [Column("CEP")]
    public string CEP { get; set; }
    
    [JsonIgnore]
    public virtual Usuario Usuario { get; set; }
}