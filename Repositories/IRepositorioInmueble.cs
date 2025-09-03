using inmobiliaria.Models;

namespace inmobiliaria.Repositories
{
 public interface IRepositorioInmueble : IRepositorio<Inmueble>
    {
      public IList<Inmueble> BuscarPorDireccion(string direccion);
      public IList<Inmueble> BuscarPorPropietario(int id);
 }
}