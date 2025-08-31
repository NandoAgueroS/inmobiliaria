using inmobiliaria.Models;

namespace inmobiliaria.Repositories
{
    public interface IRepositorioInquilino : IRepositorio<Inquilino>
    {
    
        IList<Inquilino> BuscarPorNombre(string nombre);
    }
}