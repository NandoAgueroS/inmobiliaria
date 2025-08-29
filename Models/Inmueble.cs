using System.ComponentModel.DataAnnotations;

namespace inmobiliaria.Models
{
    public class Inmueble 
    {

        public int Id { get; set; }
        [Required]
        public Tipo Tipo { get; set; }
        [Required]
        public string Uso { get; set; }
        [Required]
        public int Ambientes { get; set; }
        [Required]
        public decimal Precio { get; set; }
        [Required]
        public string Coordenadas { get; set; }
        [Required]
        public Propietario Propietario { get; set; }
        [Required]
        public bool Estado { get; set; }

        


    }
}