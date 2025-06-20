using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RegistroTecnico.Components.Models;

public class Sistemas
{
    [Key]
    public int SistemaId { get; set; }

    [Required(ErrorMessage = "Este campo es obligatorio")]
    [StringLength(500, ErrorMessage = "Máximo 500 caracteres por descripción")]
    public string Descripcion { get; set; } = null!;

    [Required(ErrorMessage = "Este campo es obligatorio")]
    public string Complejidad { get; set; } = null!;

    [Required(ErrorMessage = "Este campo es obligatorio")]
    [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Solo se permiten números positivos")]
    
    public double Coste { get; set; }
    public DateOnly Fecha { get; set; } = DateOnly.FromDateTime(DateTime.Now);

    [Required(ErrorMessage = "Este campo es obligatorio")]
    [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Solo se permiten números positivos")]
    public int? Existencia { get; set; }
}