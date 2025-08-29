namespace inmobiliaria.Repositories
{
    public interface IRepositorio<T>
    {
     int Alta(T m);
     int Baja(int id);
     int Modificacion(int id);
     IList<T> ListarTodos();
     T BuscarPorId(int id);
     }
}
