namespace inmobiliaria.Repositories
{
    public interface IRepositorio<T>
    {
     int Alta(T m);
     int Baja(int id);
     int Modificacion(T m);
     IList<T> ListarTodos();
     T BuscarPorId(int id);
     }
}
