
using inmobiliaria.Models;
using MySql.Data.MySqlClient;

namespace inmobiliaria.Repositories
{

    public class RepositorioTipo: RepositorioBase, IRepositorioTipo
    {

        public RepositorioTipo(IConfiguration configuration) : base(configuration)
        {
        }

        public int Alta(Tipo m)
        {
            int res = -1;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = $@"INSERT INTO Tipos (
                {nameof(Tipo.Descripcion)},
                {nameof(Tipo.Estado)}) VALUES (
                @Descripccion, true);
                SELECT LAST_INSERT_ID();";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Descripccion", m.Descripcion);
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
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = $@"UPDATE Tipos SET {nameof(Tipo.Estado)} = 0 WHERE IdTipo = @Id";

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

        public int Modificacion(Tipo m)
        {

            int res = -1;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = $@"UPDATE Tipos SET
                {nameof(Tipo.Descripcion)} = @Descripcion
                WHERE IdTipo = @Id;";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", m.Id);
                    command.Parameters.AddWithValue("@Descripcion", m.Descripcion);
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return res;
        }

    
    public IList<Tipo> ListarTodos()
        {
            IList<Tipo> res = new List<Tipo>();
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = $@"SELECT * FROM Tipos WHERE Estado = true;";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        res.Add(new Tipo
                        {
                            Id = reader.GetInt32("IdTipo"),
                            Descripcion = reader.GetString("Descripcion")
                        });
                    }
                }
            }
            return res;
        }

        public Tipo BuscarPorId(int id) {

            Tipo res = null;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = $@"SELECT * FROM Tipos WHERE c.IdTipo = @Id AND Estado = true";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    connection.Open();

                    var reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        res = new Tipo
                        {
                            Id = reader.GetInt32("IdTipo"),
                            Descripcion = reader.GetString("Descripcion")
                        };
                             
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
                string query = $@"UPDATE Tipos SET {nameof(Tipo.Estado)} = true WHERE IdTipo = @Id";

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