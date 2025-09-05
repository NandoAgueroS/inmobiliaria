using System.ComponentModel.DataAnnotations;

namespace inmobiliaria.Models
{
    public class Tipo
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "La descripcion es un campo obligatorio")]
        public string Descripcion { get; set; }
        public bool Estado { get; set; }
    }
}