
using inmobiliaria.Models;
using MySql.Data.MySqlClient;

namespace inmobiliaria.Repositories
{
    public class RepositorioPropietario : RepositorioBase, IRepositorioPropietario
    {
        public RepositorioPropietario(IConfiguration configuration) : base(configuration)
        {
        }

        public int Alta(Propietario p)
        {
            int res = -1;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = $@"INSERT INTO Propietarios(
                {nameof(Propietario.Nombre)},
                {nameof(Propietario.Apellido)},
                {nameof(Propietario.Dni)},
                {nameof(Propietario.Telefono)},
                {nameof(Propietario.Email)},
                {nameof(Propietario.Direccion)},
                {nameof(Propietario.Estado)}) VALUES(@Nombre, @Apellido, @Dni, @Telefono, @Email, @Direccion,true);
                SELECT LAST_INSERT_ID();";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Nombre", p.Nombre);
                    command.Parameters.AddWithValue("@Apellido", p.Apellido);
                    command.Parameters.AddWithValue("@Dni", p.Dni);
                    command.Parameters.AddWithValue("@Telefono", p.Telefono);
                    command.Parameters.AddWithValue("@Email", p.Email);
                    command.Parameters.AddWithValue("@Direccion", p.Direccion);
                    connection.Open();
                    res = Convert.ToInt32(command.ExecuteScalar());
                    p.Id=res;
                    connection.Close();
                }
                return res;

            }
        }
        public int Baja(int Id)
        {
            int res = -1;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = $@"UPDATE Propietarios SET {nameof(Propietario.Estado)}= 0 WHERE IdPropietario=@Id";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", Id);
                    connection.Open();

                    res = (command.ExecuteNonQuery());
                    connection.Close();
                }
            }

            return res;

        }
        public int Modificacion(Propietario p)
        {
            int res = -1;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = $@"UPDATE Propietarios SET 
             {nameof(Propietario.Nombre)}=@Nombre,
             {nameof(Propietario.Apellido)}=@Apellido,
             {nameof(Propietario.Dni)}=@Dni,
             {nameof(Propietario.Telefono)}=@Telefono,
             {nameof(Propietario.Email)}=@Email,
             {nameof(Propietario.Direccion)}=@Direccion
            WHERE IdPropietario = @Id;";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Nombre", p.Nombre);
                    command.Parameters.AddWithValue("@Apellido", p.Apellido);
                    command.Parameters.AddWithValue("@Dni", p.Dni);
                    command.Parameters.AddWithValue("@Telefono", p.Telefono);
                    command.Parameters.AddWithValue("@Email", p.Email);
                    command.Parameters.AddWithValue("@Direccion", p.Direccion);
                    command.Parameters.AddWithValue("@Id", p.Id);
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return res;
        }
        public IList<Propietario> ListarTodos()
        {
            IList<Propietario> res = new List<Propietario>();
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = @$"SELECT * FROM Propietarios WHERE Estado = true";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    connection.Open();
                    MySqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        res.Add(new Propietario
                        {
                            Id = reader.GetInt32("IdPropietario"),
                            Nombre = reader.GetString(nameof(Propietario.Nombre)),
                            Apellido = reader.GetString(nameof(Propietario.Apellido)),
                            Dni = reader.GetString(nameof(Propietario.Dni)),
                            Telefono = reader.GetString(nameof(Propietario.Telefono)),
                            Email = reader.GetString(nameof(Propietario.Email)),
                            Direccion = reader.GetString(nameof(Propietario.Direccion)),


                        });
                    }
                }

            }
            return res;
        }

        public Propietario BuscarPorId(int id)
        {

            Propietario res = null;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = $@"SELECT * FROM Propietarios WHERE IdPropietario = @Id AND Estado = true";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    connection.Open();

                    var reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        res = new Propietario
                        {
                            Id = reader.GetInt32("IdPropietario"),
                            Nombre = reader.GetString(nameof(Propietario.Nombre)),
                            Apellido = reader.GetString(nameof(Propietario.Apellido)),
                            Dni = reader.GetString(nameof(Propietario.Dni)),
                            Email = reader.GetString(nameof(Propietario.Email)),
                            Telefono = reader.GetString(nameof(Propietario.Telefono)),
                            Direccion = reader.GetString(nameof(Propietario.Direccion))
                        };
                    }
                }
            }
            return res;
        }
        public IList<Propietario> BuscarPorNombre(string nombre)
        {
            IList<Propietario> res = new List<Propietario>();
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = $@"SELECT * FROM Propietarios 
                WHERE {nameof(Propietario.Estado)}= true AND Nombre LIKE @nombre;";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@nombre", $"%{nombre}%");
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        res.Add(new Propietario
                        {
                            Id = reader.GetInt32("IdPropietario"),
                            Nombre = reader.GetString(nameof(Propietario.Nombre)),
                            Apellido = reader.GetString(nameof(Propietario.Apellido)),
                            Dni = reader.GetString(nameof(Propietario.Dni)),
                            Email = reader.GetString(nameof(Propietario.Email)),
                            Telefono = reader.GetString(nameof(Propietario.Telefono)),
                            Direccion = reader.GetString(nameof(Propietario.Direccion)),
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
                string query = $@"UPDATE Propietarios SET {nameof(Propietario.Estado)} = true WHERE IdPropietario = @Id";

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