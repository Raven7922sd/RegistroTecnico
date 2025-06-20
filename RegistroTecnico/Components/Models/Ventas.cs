using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RegistroTecnico.Components.Models;
public class Ventas
{
    [Key]
    public int VentaId { get; set; }

    public DateOnly Fecha { get; set; } = DateOnly.FromDateTime(DateTime.Now);

    [Required(ErrorMessage = "Debe seleccionar un cliente")]
    [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un cliente")]
    public int ClienteId { get; set; }
   
    public Clientes Cliente { get; set; } = null!;

    [InverseProperty("Venta")]
    public virtual ICollection<VentasDetalles> VentasDetalles { get; set; } = new List<VentasDetalles>();
}