using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace inmobiliaria.Models
{
    public class Tipo
    {
        [BindNever]
        public int Id { get; set; }
        
        [Required(ErrorMessage = "La descripcion es un campo obligatorio")]
        public string Descripcion { get; set; }
        public bool Estado { get; set; }
    }
}