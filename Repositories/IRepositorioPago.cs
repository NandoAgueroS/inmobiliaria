using inmobiliaria.Models;

namespace inmobiliaria.Repositories
{
    public interface IRepositorioPago : IRepositorio<Pago>
    {
        public IList<Pago> BuscarPagoDeInquilino(int idInquilino);
        public IList<Pago> BuscarPagoPorContrato(int idContrato);

        public IList<Pago> BuscarPagoPorFecha(DateOnly fechaActual);
        public Pago BuscarUltimoPago(int idContrato);
        public IList<Pago> ListarPorInquilino(int idInquilino);
        public int ContarPagosMensuales(int idContrato);
        public int ObtenerUltimoNumeroPago(int idContrato);
        public int Baja(int id, int anuladoPor);
    }
}
