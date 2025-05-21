using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RegistroTecnico.Components.Models;

public class Clientes
{
    [Key]
    public int ClienteId { get; set; }
    public DateOnly FechaIngreso { get; set; } = DateOnly.FromDateTime(DateTime.Now);

    [Required(ErrorMessage = "Este campo es obligatorio")]
    public string ClienteNombre { get; set; } = null!;

    [Required(ErrorMessage = "Este campo es obligatorio")]
    public string DireccionCliente { get; set; } = null!;

    [StringLength(11, MinimumLength = 9, ErrorMessage = "El RNC debe tener entre 9 y 11 caracteres.")]
    public string Rnc { get; set; }

    [Range(1,100000,ErrorMessage ="El límite de crédito es de 0 hasta 100000.")]
    public double LimiteCredito { get; set; }

    [ForeignKey("TecnicoId")]
    public int TenicoId { get; set; }

    public virtual Tecnicos Tecnicos { get; set; }
}
