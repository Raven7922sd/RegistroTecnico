using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RegistroTecnico.Components.Models;

public class Clientes
{
    [Key]
    public int ClienteId { get; set; }
    public DateOnly FechaIngreso { get; set; } = DateOnly.FromDateTime(DateTime.Now);

    [Required(ErrorMessage = "Este campo es obligatorio")]
    [RegularExpression(@"^[^\d]*$", ErrorMessage = "Este campo no puede contener números")]
    public string ClienteNombre { get; set; } = null!;

    [Required(ErrorMessage = "Este campo es obligatorio")]
    public string DireccionCliente { get; set; } = null!;

    [Required(ErrorMessage ="Este campo es obligatorio")]
    [StringLength(11, MinimumLength = 9, ErrorMessage = "El RNC debe tener valores numéricos entre 9 y 11 caracteres.")]
    public string Rnc { get; set; }
    
    [Required(ErrorMessage ="Este campo solo acepta valores numéricos")]
    [Range(1,100000,ErrorMessage ="El límite de crédito es de 0 hasta 100000.")]
    public double LimiteCredito { get; set; }

    [ForeignKey("TecnicoId")]
    public int TecnicoId { get; set; }
    public virtual Tecnicos Tecnico { get; set; }
}
