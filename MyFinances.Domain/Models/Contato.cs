using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MyFinances.Domain.Models;

[Table("CONTATO")]
public class Contato
{
    [Key]
    [Required]
    [Column("ID")]
    public Guid Id { get; set; }

    [Column("TELEFONE_FIXO")]
    public string TelefoneFixo { get; set; }
    
    [Column("CELULAR")]
    public string Celular { get; set; }
    
    [Required, Column("ID_USUARIO")]
    public Guid IdUsuario { get; set; }
    
    [JsonIgnore]
    public virtual Usuario Usuario { get; set; }
    
}