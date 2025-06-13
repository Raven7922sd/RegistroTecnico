using System.ComponentModel.DataAnnotations;

namespace RegistroTecnico.Components.Models;

public class Sistemas
{
    [Key]
    public int SistemaId { get; set; }

    [Required(ErrorMessage = "Este campo es obligatorio")]
    [StringLength(50, ErrorMessage = "Máximo 500 caracteres por descripción")]
    public string Descripcion { get; set; } = null!;

    [Required(ErrorMessage = "Este campo es obligatorio")]
    [Range(0,100,ErrorMessage ="El rango es de '0-100'")]
    public int Complejidad { get; set; }

    public DateOnly Fecha { get; set; } = DateOnly.FromDateTime(DateTime.Now);
}
