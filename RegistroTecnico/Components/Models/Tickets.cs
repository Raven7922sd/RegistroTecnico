using System.ComponentModel.DataAnnotations;

namespace RegistroTecnico.Components.Models;
public class Tickets
{
    [Key]
    public int TicketId { get; set; }
    public DateOnly Fecha { get; set; }= DateOnly.FromDateTime(DateTime.Now);
    [Required(ErrorMessage = "Este campo es obligatorio")]
    public double TiempoInvertido { get;set; }

    [Required(ErrorMessage = "Este campo es obligatorio")]
    public string Prioridad { get; set; }
    [Required(ErrorMessage = "Este campo es obligatorio")]
    public string Asunto { get; set; }
    [Required(ErrorMessage = "Este campo es obligatorio")]
    public string Descripcion{ get; set; }
    [Required(ErrorMessage = "Este campo es obligatorio")]
    public int TecnicoId { get; set; }
    [Required(ErrorMessage = "Este campo es obligatorio")]
    public int ClienteId{ get; set; }
    public Clientes Cliente {  get; set; }
    public Tecnicos Tecnico { get; set; }
}
