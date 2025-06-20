using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RegistroTecnico.Components.Models;

public class VentasDetalles
{
    [Key]
    public int DetalleId { get; set; }

    public double Monto { get; set; }

    public int Cantidad { get; set; }

    public double ValorCobrado { get; set; }

    public int SistemaId { get; set; }
    [ForeignKey("SistemaId")]
    public virtual Sistemas Sistema { get; set; } = null!;

    public int VentaId { get; set; }
    [ForeignKey("VentaId")]
    [InverseProperty("VentasDetalles")]
    public virtual Ventas Venta { get; set; } = null!;
}