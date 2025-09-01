using inmobiliaria.Models;

namespace inmobiliaria.Repositories
{

    public interface IRepositorioPropietario : IRepositorio<Propietario>
    {
        IList<Propietario> BuscarPorNombre(string nombre);
    }
}