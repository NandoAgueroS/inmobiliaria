using System.ComponentModel.DataAnnotations;

namespace inmobiliaria.Models
{
    public class CancelarContratoDTO
    {
        public int IdContrato { get; set; }
        [Required]
        public DateOnly FechaCancelacion { get; set; }
    }
}
