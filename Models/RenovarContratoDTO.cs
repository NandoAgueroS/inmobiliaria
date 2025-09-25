namespace inmobiliaria.Models
{
    public class RenovarContratoDTO
    {
        public int IdOriginal { get; set; }
        public decimal NuevoMonto { get; set; }

        public DateOnly FechaDesde { get; set; }

        public DateOnly FechaHasta { get; set; }
    }
}
