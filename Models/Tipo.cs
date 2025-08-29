using System.ComponentModel.DataAnnotations;

namespace inmobiliaria.Models
{
    public class Tipo 
    {
        public int Id {get;set;}
        [Required]
        public string Descripcion {get;set;}
    }
}