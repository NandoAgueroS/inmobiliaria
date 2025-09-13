using inmobiliaria.Models;

namespace inmobiliaria.Repositories
{
    public interface IRepositorioPago : IRepositorio<Pago>
    {
        public IList<Pago> BuscarPagoDeInquilino(int idInquilino);
        public IList<Pago> BuscarPagoPorContrato(int idContrato);

        public IList<Pago> BuscarPagoPorFecha(DateOnly fechaActual);

        


    }
}