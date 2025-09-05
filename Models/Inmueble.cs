using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace inmobiliaria.Models
{
    public class Inmueble 
    {

        [BindNever]
        public int Id { get; set; }

        [Required(ErrorMessage = "Es obligatorio por el tipo")]
        public int IdTipo{ get; set; }

        public Tipo? Tipo { get; set; }

        [Required(ErrorMessage = "El uso es un campo obligatorio")]
        public string Uso { get; set; }

        [Required(ErrorMessage = "Es obligatorio poner la cantidad de ambientes")]
        public int Ambientes { get; set; }

         [Required(ErrorMessage = "La direccion es un campo obligatorio")] public string Direccion { get; set; }

        [Required(ErrorMessage = "El precio es un campo obligatorio")]
        public decimal Precio { get; set; }

        [Required(ErrorMessage = "Las coordenadas son obligatorias")]
        public string Coordenadas { get; set; }

        [Required(ErrorMessage = "Es un campo obligatorio el propietario")]
        public int IdPropietario { get; set; }

        public Propietario Propietario { get; set; }
        
        public bool Estado { get; set; }

        


    }
}