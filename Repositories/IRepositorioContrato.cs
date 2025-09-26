using inmobiliaria.Models;

namespace inmobiliaria.Repositories
{
    public interface IRepositorioContrato : IRepositorio<Contrato>
    {

        Contrato BuscarActualPorInquilino(int idInquilino);
        IList<Contrato> ListarPorInmueble(int idInmueble);
        int Baja(int id, int anuladoPor);
    }
}
