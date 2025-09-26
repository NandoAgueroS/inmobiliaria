using inmobiliaria.Models;

namespace inmobiliaria.Repositories
{
    public interface IRepositorioInmueble : IRepositorio<Inmueble>
    {
        public IList<Inmueble> BuscarPorDireccion(string direccion);
        public IList<Inmueble> BuscarPorPropietario(int id);
        public IList<Inmueble> ListarPorDisponible(bool disponibles);
        public IList<Inmueble> ListarDesocupados(DateOnly fechaDesde, DateOnly fechaHasta);
        public bool VerificarDesocupado(DateOnly fechaDesde, DateOnly fechaHasta, int id);
        public bool VerificarDisponible(int id);
    }
}
