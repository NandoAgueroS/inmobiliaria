using inmobiliaria.Models;
using MySql.Data.MySqlClient;

namespace inmobiliaria.Repositories
{
    public class RepositorioUsuario : RepositorioBase, IRepositorioUsuario
    {
        public RepositorioUsuario(IConfiguration configuration) : base(configuration)
        {
        }

        public int Alta(Usuario m)
        {
            int res = -1;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = $@"INSERT INTO Usuarios (
                
                
                {nameof(Usuario.Nombre)},
                {nameof(Usuario.Apellido)},
                {nameof(Usuario.Email )},
                {nameof(Usuario.Clave)},
                {nameof(Usuario.Avatar)},
                {nameof(Usuario.Rol)},
                {nameof(Usuario.Estado)}) VALUES (
                @Nombre, @Apellido, @Email, @Clave, @Avatar, @Rol, true);
                SELECT LAST_INSERT_ID();";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Nombre", m.Nombre);
                    command.Parameters.AddWithValue("@Apellido", m.Apellido);
                    command.Parameters.AddWithValue("@Email", m.Email);
                    command.Parameters.AddWithValue("@Clave", m.Clave);
                    if (String.IsNullOrEmpty(m.Avatar))
                        command.Parameters.AddWithValue("@Avatar", "");
                    else
                        command.Parameters.AddWithValue("@Avatar", m.Avatar);
                    command.Parameters.AddWithValue("@Rol",m.Rol);
                    connection.Open();
                    res = Convert.ToInt32(command.ExecuteScalar());
                    m.Id = res;
                    connection.Close();
                }
                
            }
            return res;
        }

        public int Baja(int id)
        {
            int res = -1;
            using(MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = $@"UPDATE Usuarios SET {nameof(Usuario.Estado)} = false WHERE IdUsuario = @Id";

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

        public Usuario BuscarPorId(int id)
        {
            Usuario res = null;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = $@"SELECT * FROM Usuarios WHERE IdUsuario = @Id";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    connection.Open();
                    var reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        res = new Usuario
                        {
                            Id = reader.GetInt32("IdUsuario"),
                            Nombre = reader.GetString(nameof(Usuario.Nombre)),
                            Apellido = reader.GetString(nameof(Usuario.Apellido)),
                            Email = reader.GetString(nameof(Usuario.Email)),
                            Clave = reader.GetString(nameof(Usuario.Clave)),
                            Avatar = reader.GetString(nameof(Usuario.Avatar)),
                            Rol = reader.GetInt32(nameof(Usuario.Rol)),
                            Estado = reader.GetBoolean(nameof(Usuario.Estado))
                        };

                    }
                    connection.Close();
                }
            }
            return res;
        }

        public IList<Usuario> ListarTodos()
        {
            IList<Usuario> res = new List<Usuario>();
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = $@"SELECT * FROM Usuarios WHERE Estado = true";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        res.Add(new Usuario
                        {
                            Id = reader.GetInt32("IdUsuario"),
                            Nombre = reader.GetString(nameof(Usuario.Nombre)),
                            Apellido = reader.GetString(nameof(Usuario.Apellido)),
                            Email = reader.GetString(nameof(Usuario.Email)),
                            Clave = reader.GetString(nameof(Usuario.Clave)),
                            Avatar = reader.GetString(nameof(Usuario.Avatar)),
                            Rol = reader.GetInt32(nameof(Usuario.Rol)),
                            Estado = reader.GetBoolean(nameof(Usuario.Estado))



                        });
                    }
                    connection.Close();
                }
            }
            return res;
        }

        public int Modificacion(Usuario m)
        {
           int res = -1;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = $@"UPDATE Usuarios SET
                {nameof(Usuario.Nombre)} = @Nombre ,
                {nameof(Usuario.Apellido)} = @Apellido ,
                {nameof(Usuario.Email)} = @Email,
                {nameof(Usuario.Clave)} = @Clave,
                {nameof(Usuario.Avatar)} = @Avatar,
                {nameof(Usuario.Rol)} = @Rol
                WHERE IdUsuario = @Id;";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Nombre", m.Nombre);
                    command.Parameters.AddWithValue("@Apellido", m.Apellido);
                    command.Parameters.AddWithValue("@Email", m.Email);
                    command.Parameters.AddWithValue("@Clave", m.Clave);
                    command.Parameters.AddWithValue("@Avatar", m.Avatar);
                    command.Parameters.AddWithValue("@Rol", m.Rol);
                    command.Parameters.AddWithValue("@Id", m.Id);

                    connection.Open();
                    res = command.ExecuteNonQuery();
                    connection.Close();

                }
           }
            return res;
        }

        public Usuario ObtenerPorEmail(string email)
        {
            Usuario res = null;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = $@"SELECT * FROM Usuarios WHERE Email = @Email AND Estado = true";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Email", email);
                    connection.Open();
                    var reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        res = new Usuario
                        {
                            Id = reader.GetInt32("IdUsuario"),
                            Nombre = reader.GetString(nameof(Usuario.Nombre)),
                            Apellido = reader.GetString(nameof(Usuario.Apellido)),
                            Email = reader.GetString(nameof(Usuario.Email)),
                            Clave = reader.GetString(nameof(Usuario.Clave)),
                            Avatar = reader.GetString(nameof(Usuario.Avatar)),
                            Rol = reader.GetInt32(nameof(Usuario.Rol)),
                            Estado = reader.GetBoolean(nameof(Usuario.Estado))

                        };
                    }
                    connection.Close();
                }
            }
            return res;
        }

        public int Reactivar(int id)
        {
             int res = -1;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = $@"UPDATE Usuarios SET {nameof(Usuario.Estado)} = true WHERE IdUsuario = @Id";

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