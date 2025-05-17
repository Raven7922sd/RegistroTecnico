using System.ComponentModel.DataAnnotations;

namespace RegistroTecnico.Components.Models
{
    public class Tecnicos
    {
        [Key]
        public int TecnicoId { get; set; }


        [Required(ErrorMessage = "Este campo es obligatorio")]
        [RegularExpression(@"^[^\d]*$",ErrorMessage ="Este campo no puede contener números")]
        public string TecnicoNombre { get; set; } = null!;

        [Range(0.1, 50000, ErrorMessage = "El sueldo debe se mayor que '0' y menor a '50,000'")]
        [RegularExpression(@"^\d{1,5}(\.\d{1,2})?$", ErrorMessage = "Máximo 2 decimales permitidos")]
        public double SueldoHora { get; set; }
            
      


    }
}
