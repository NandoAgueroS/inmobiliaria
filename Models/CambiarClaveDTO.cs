using System.ComponentModel.DataAnnotations;

namespace inmobiliaria.Models
{
    public class CambiarClaveDTO
    {
        [Required]
        public int IdUsuario { get; set; }
        [Required]
        public string Antigua { get; set; }
        [Required]
        public string Nueva { get; set; }
        [Required]
        public string NuevaConfirmada { get; set; }
    }
}
