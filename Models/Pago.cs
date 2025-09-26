using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;

namespace inmobiliaria.Models
{
   
    public class Pago
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "Numero de pago es un campo obligatorio")]
        public string NumeroPago { get; set; }

        [Required(ErrorMessage = "El concepto es un campo obligatorio")]
        public string Concepto { get; set; }

        [Required(ErrorMessage = "El monto es un campo obligatorio")]
        public decimal Monto { get; set; }

        [Required(ErrorMessage = "El Fecha es un campo obligatorio")]
        public DateOnly Fecha { get; set; } = DateOnly.FromDateTime(DateTime.Now);

        [Required(ErrorMessage = "El Contrato es un campo obligatorio")]
        public int IdContrato { get; set; }

        public Contrato? Contrato { get; set; }

        [DataType(DataType.Date)]
        public DateOnly? CorrespondeAMes { get; set; }

        public int CreadoPor { get; set; }


        public int? AnuladoPor { get; set; }

        public bool Estado { get; set; }


    }
}
