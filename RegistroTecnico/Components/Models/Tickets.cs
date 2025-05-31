using System.ComponentModel.DataAnnotations;
namespace RegistroTecnico.Components.Models;
public class Tickets
{
    [Key]
    public int TicketId { get; set; }
    public DateOnly Fecha { get; set; } = DateOnly.FromDateTime(DateTime.Now);

    [Required(ErrorMessage = "Este campo es obligatorio")]
    [Range(0.01, 3000, ErrorMessage = "El tiempo invertido debe ser un número positivo entre 1 y 3000")]
    public double TiempoInvertido { get; set; }

    [Required(ErrorMessage = "Este campo es obligatorio")]
    public string Prioridad { get; set; } = null!;

    [Required(ErrorMessage = "La prioridad es obligatoria")]
    [StringLength(220, ErrorMessage = "El asunto no puede tener más de 500 caracteres")]
    public string Asunto { get; set; } = null!;

    [Required(ErrorMessage = "Este campo es obligatorio")]
    [StringLength(220, ErrorMessage = "La descripción no puede tener más de 500 caracteres")]
    public string Descripcion { get; set; } = null!;

    [Required(ErrorMessage = "Debe seleccionar un técnico")]
    [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un técnico")]
    public int TecnicoId { get; set; }

    [Required(ErrorMessage = "Debe seleccionar un cliente")]
    [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un cliente")]
    public int ClienteId { get; set; }

    public Clientes Cliente { get; set; } = null!;
    public Tecnicos Tecnico { get; set; } = null!;
}