using System.ComponentModel.DataAnnotations;

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

    public DateOnly Fecha { get; set; } = DateOnly.FromDateTime(DateTime.Now);
}