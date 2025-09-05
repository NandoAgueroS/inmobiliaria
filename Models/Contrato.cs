using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace inmobiliaria.Models
{
    public class Contrato
    {
        [BindNever]
        public int Id { get; set; }

        [Required(ErrorMessage = "El inquilino es obligatorio")]
        public int IdInquilino { get; set; }

        [Required(ErrorMessage = "El inmueble es obligatorio")]
        public int IdInmueble { get; set; }

        public Inquilino? Inquilino { get; set; }

        public Inmueble? Inmueble { get; set; }
        
        [Required(ErrorMessage = "El monto es obligatorio")]
        public decimal Monto { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Inicio")]
        [Required(ErrorMessage = "La fecha de inicio es obligatoria")]
        public DateOnly FechaDesde { get; set; }


        [DataType(DataType.Date)]
        [Display(Name = "Fin")]
        [Required(ErrorMessage = "La fecha de fin es obligatoria")]
        public DateOnly FechaHasta { get; set; }

        public bool Estado { get; set; }
    }
}