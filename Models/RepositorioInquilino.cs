
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
                {nameof(Inquilino.Estado)}) VALUES(@Nombre, @Apellido, @Telefono, @Dni, @Email, 1);
                SELECT LAST_INSERT_ID();";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Nombre", i.Nombre);
                    command.Parameters.AddWithValue("@Apellido", i.Apellido);
                    command.Parameters.AddWithValue("@Dni", i.Dni);
                    command.Parameters.AddWithValue("@Telefono", i.Telefono);
                    command.Parameters.AddWithValue("@Email", i.Email);
                    connection.Open();
                    res = Convert.ToInt32(command.ExecuteScalar());
                    connection.Close();
                }
            }
            return res;
        }

        public int Baja(int id)
        {
            int res = -1;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = $@"UPDATE Inquilinos SET {nameof(Inquilino.Estado)} = 0 WHERE IdInquilino = @Id";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return res;
        }

        public int Modificacion(Inquilino i)
        {

            int res = -1;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = $@"UPDATE Inquilinos SET
                {nameof(Inquilino.Nombre)} = @Nombre,
                {nameof(Inquilino.Apellido)} = @Apellido, 
                {nameof(Inquilino.Dni)} = @Dni,
                {nameof(Inquilino.Telefono)} = @Telefono,
                {nameof(Inquilino.Email)} = @Email
                WHERE IdInquilino = @Id;";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Nombre", i.Nombre);
                    command.Parameters.AddWithValue("@Apellido", i.Apellido);
                    command.Parameters.AddWithValue("@Dni", i.Dni);
                    command.Parameters.AddWithValue("@Telefono", i.Telefono);
                    command.Parameters.AddWithValue("@Email", i.Email);
                    command.Parameters.AddWithValue("@Id", i.Id);
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return res;
        }

    
    public IList<Inquilino> ListarTodos()
        {
            IList<Inquilino> res = new List<Inquilino>();
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = $@"SELECT * FROM Inquilinos WHERE Estado = true;";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        res.Add(new Inquilino
                        {
                            Id = reader.GetInt32("IdInquilino"),
                            Nombre = reader.GetString(nameof(Inquilino.Nombre)),
                            Apellido = reader.GetString(nameof(Inquilino.Apellido)),
                            Dni = reader.GetString(nameof(Inquilino.Dni)),
                            Email = reader.GetString(nameof(Inquilino.Email)),
                            Telefono = reader.GetString(nameof(Inquilino.Telefono)),
                            Estado = false
                        });
                    }
                }
            }
            return res;
        }

        public Inquilino BuscarPorId(int id) {

            Inquilino res = null;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = $@"SELECT * FROM Inquilinos WHERE IdInquilino = @Id AND Estado = true";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    connection.Open();

                    var reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        res = new Inquilino
                        {
                            Id = reader.GetInt32("IdInquilino"),
                            Nombre = reader.GetString(nameof(Inquilino.Nombre)),
                            Apellido = reader.GetString(nameof(Inquilino.Apellido)),
                            Dni = reader.GetString(nameof(Inquilino.Dni)),
                            Email = reader.GetString(nameof(Inquilino.Email)),
                            Telefono = reader.GetString(nameof(Inquilino.Telefono))
                        };
                    }
                }
            }
            return res;
        }
    }
}