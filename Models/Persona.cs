using System.ComponentModel.DataAnnotations;

namespace inmobiliaria.Models
{
    public class Persona
    {

        [Required]
        public int Id { get; set; }

        [Required]
        public string? Nombre { get; set; }

        [Required]
        public string? Apellido { get; set; }

        [Required]
        public string? Dni { get; set; }

        [Required]
        public string? Telefono { get; set; }

        [Required]
        public string? Email { get; set; }

        public Boolean Estado { get; set; }

    }   
}