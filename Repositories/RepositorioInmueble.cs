using inmobiliaria.Models;

namespace inmobiliaria.Repositories
{
    public class RepositorioInmueble : RepositorioBase, IRepositorioInmueble
    {
        public RepositorioInmueble(IConfiguration configuration) : base(configuration)
        {

        }

        public int Alta(Inmueble m)
        {
            return 0;
           
        }

        public int Baja(int id)
        {
            return 0;
            
        }

        public Inmueble BuscarPorId(int id)
        {
            return null;
        }

        public IList<Inmueble> ListarTodos()
        {
            return null;
        }

        public int Modificacion(Inmueble m)
        {
            return 0;
        }
    }


}