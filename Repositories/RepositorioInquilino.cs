
using inmobiliaria.Models;
using MySql.Data.MySqlClient;

namespace inmobiliaria.Repositories
{

    public class RepositorioInquilino : RepositorioBase, IRepositorioInquilino
    {
        public RepositorioInquilino(IConfiguration configuration) : base(configuration)
        {
        }

        public int Alta(Inquilino m)
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
                    command.Parameters.AddWithValue("@Nombre", m.Nombre);
                    command.Parameters.AddWithValue("@Apellido", m.Apellido);
                    command.Parameters.AddWithValue("@Dni", m.Dni);
                    command.Parameters.AddWithValue("@Telefono", m.Telefono);
                    command.Parameters.AddWithValue("@Email", m.Email);
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

        public int Modificacion(Inquilino m)
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
                    command.Parameters.AddWithValue("@Nombre", m.Nombre);
                    command.Parameters.AddWithValue("@Apellido", m.Apellido);
                    command.Parameters.AddWithValue("@Dni", m.Dni);
                    command.Parameters.AddWithValue("@Telefono", m.Telefono);
                    command.Parameters.AddWithValue("@Email", m.Email);
                    command.Parameters.AddWithValue("@Id", m.Id);
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

        public Inquilino BuscarPorId(int id)
        {

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

        public IList<Inquilino> BuscarPorNombre(string nombre)
        {
            IList<Inquilino> res = new List<Inquilino>();
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = $@"SELECT * FROM Inquilinos 
                WHERE {nameof(Inquilino.Estado)}= true AND Nombre LIKE @nombre;";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@nombre", $"%{nombre}%");
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
                        });
                    }
                }
            }
            return res;
        }
        public int Reactivar(int id)
        {
            int res = -1;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = $@"UPDATE Inquilinos SET {nameof(Inquilino.Estado)} = true WHERE IdInquilino = @Id";

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
    }
}