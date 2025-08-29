using System.ComponentModel.DataAnnotations;

namespace inmobiliaria.Models
{
    public class Persona
    {

        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string? Nombre { get; set; }

        [Required(ErrorMessage = "El apellido es obligatorio")]
        public string? Apellido { get; set; }

        [Required(ErrorMessage = "El dni es obligatorio")]
        public string? Dni { get; set; }

        [Required(ErrorMessage = "El telefono es obligatorio")]
        public string? Telefono { get; set; }

        [Required(ErrorMessage = "El email es obligatorio")]
        public string? Email { get; set; }

        public Boolean Estado { get; set; }

    }   
}