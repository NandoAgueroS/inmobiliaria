using inmobiliaria.Repositories;
using inmobiliaria.Models;

namespace inmobiliaria.Repositories
{
    public interface IRepositorioUsuario : IRepositorio<Usuario>
    {
        Usuario ObtenerPorEmail(string email);
    }
}