
using MySql.Data.MySqlClient;

namespace inmobiliaria.Models
{

    public class RepositorioInquilino : RepositorioBase
    {
        public RepositorioInquilino(IConfiguration configuration) : base(configuration)
        {
        }

        public int Alta(Inquilino i)
        {
            int res = -1;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = $@"INSERT INTO Inquilinos (
                {nameof(Inquilino.Nombre)},
                {nameof(Inquilino.Apellido)}, 
                {nameof(Inquilino.Dni)},
                {nameof(Inquilino.Telefono)},
                {nameof(Inquilino.Email)},
                {nameof(Inquilino.Estado)}) VALUES(@nombre, @apellido, @telefono, @dni, @email, 1);
                SELECT LAST_INSERT_ID();";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@nombre", i.Nombre);
                    command.Parameters.AddWithValue("@apellido", i.Apellido);
                    command.Parameters.AddWithValue("@dni", i.Dni);
                    command.Parameters.AddWithValue("@telefono", i.Telefono);
                    command.Parameters.AddWithValue("@email", i.Email);
                    connection.Open();
                    res = Convert.ToInt32(command.ExecuteScalar());
                }
            }
            return res;
        }

        public int Baja(Inquilino i)
        {
            throw new NotImplementedException();
        }

        public int Modificacion(Inquilino i)
        {
            throw new NotImplementedException();
        }
    }
}