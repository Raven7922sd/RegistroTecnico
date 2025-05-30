using System.ComponentModel.DataAnnotations;

namespace RegistroTecnico.Components.Models;
public class Tickets
{
    [Key]
    public int TicketId { get; set; }
    public DateOnly Fecha { get; set; }= DateOnly.FromDateTime(DateTime.Now);
    [Required(ErrorMessage = "Este campo es obligatorio")]
    [Range(0.01, double.MaxValue, ErrorMessage = "El tiempo invertido debe ser un número positivo")]
    public double TiempoInvertido { get; set; }

    [Required(ErrorMessage = "Este campo es obligatorio")]
    public string Prioridad { get; set; } = null!;

    [Required(ErrorMessage = "La prioridad es obligatoria")]
    public string Asunto { get; set; } = null!;

    [Required(ErrorMessage = "Este campo es obligatorio")]
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