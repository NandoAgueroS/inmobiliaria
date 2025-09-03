
using inmobiliaria.Models;
using MySql.Data.MySqlClient;

namespace inmobiliaria.Repositories
{

    public class RepositorioContrato : RepositorioBase, IRepositorioContrato
    {
        private readonly IRepositorioInquilino repositorioInquilino;

        private readonly IRepositorioInmueble repositorioInmueble;

        public RepositorioContrato(IRepositorioInquilino repositorioInquilino, IRepositorioInmueble repositorioInmueble, IConfiguration configuration) : base(configuration)
        {
            this.repositorioInquilino = repositorioInquilino;
            this.repositorioInmueble = repositorioInmueble;
        }

        public int Alta(Contrato m)
        {
            int res = -1;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = $@"INSERT INTO Contratos (
                {nameof(Contrato.IdInquilino)},
                {nameof(Contrato.IdInmueble)}, 
                {nameof(Contrato.Monto)},
                {nameof(Contrato.FechaDesde)},
                {nameof(Contrato.FechaHasta)},
                {nameof(Contrato.Estado)}) VALUES (
                @IdInquilino, @IdInmueble, @Monto, @FechaDesde, @FechaHasta, true);
                SELECT LAST_INSERT_ID();";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IdInquilino", m.IdInquilino);
                    command.Parameters.AddWithValue("@IdInmueble", m.IdInmueble);
                    command.Parameters.AddWithValue("@Monto", m.Monto);
                    command.Parameters.AddWithValue("@FechaDesde", m.FechaDesde.ToString("yyyy-MM-dd"));
                    command.Parameters.AddWithValue("@FechaHasta", m.FechaHasta.ToString("yyyy-MM-dd"));
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
                string query = $@"UPDATE Contratos SET {nameof(Contrato.Estado)} = 0 WHERE IdContrato = @Id";

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

        public int Modificacion(Contrato m)
        {

            int res = -1;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = $@"UPDATE Contratos SET
                {nameof(Contrato.IdInquilino)} = @IdInquilino,
                {nameof(Contrato.IdInmueble)} = @IdInmueble,
                {nameof(Contrato.Monto)} = @Monto,
                {nameof(Contrato.FechaDesde)} = @FechaDesde,
                {nameof(Contrato.FechaHasta)} = @FechaHasta
                WHERE IdContrato = @Id;";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", m.Id);
                    command.Parameters.AddWithValue("@IdInquilino", m.IdInquilino);
                    command.Parameters.AddWithValue("@IdInmueble", m.IdInmueble);
                    command.Parameters.AddWithValue("@Monto", m.Monto);
                    command.Parameters.AddWithValue("@FechaDesde", m.FechaDesde.ToString("yyyy-MM-dd"));
                    command.Parameters.AddWithValue("@FechaHasta", m.FechaHasta.ToString("yyyy-MM-dd"));
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return res;
        }

    
    public IList<Contrato> ListarTodos()
        {
            IList<Contrato> res = new List<Contrato>();
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = $@"SELECT * FROM Contratos WHERE Estado = true;";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        res.Add(new Contrato
                        {
                            Id = reader.GetInt32("IdContrato"),
                            IdInquilino = reader.GetInt32(nameof(Contrato.IdInquilino)),
                            IdInmueble = reader.GetInt32(nameof(Contrato.IdInmueble)),
                            Monto = reader.GetDecimal(nameof(Contrato.Monto)),
                            FechaDesde = DateOnly.FromDateTime(reader.GetDateTime (nameof(Contrato.FechaDesde))),
                            FechaHasta = DateOnly.FromDateTime(reader.GetDateTime(nameof(Contrato.FechaHasta))),
                            Estado = reader.GetBoolean(nameof(Contrato.Estado))
                        });
                    }
                }
            }
            return res;
        }

        public Contrato BuscarPorId(int id) {

            Contrato res = null;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = $@"SELECT * FROM Contratos c JOIN Inquilinos iq ON c.IdInquilino = iq.IdInquilino JOIN Inmuebles im ON c.IdInmueble = im.IdInmueble JOIN Tipos t ON im.IdTipo = im.IdTipo WHERE c.IdContrato = @Id AND c.Estado = true";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    connection.Open();

                    var reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        res = new Contrato
                        {
                            Id = reader.GetInt32("IdContrato"),
                            IdInquilino = reader.GetInt32(nameof(Contrato.IdInquilino)),
                            IdInmueble = reader.GetInt32(nameof(Contrato.IdInmueble)),
                            Monto = reader.GetDecimal(nameof(Contrato.Monto)),
                            FechaDesde = DateOnly.FromDateTime(reader.GetDateTime(nameof(Contrato.FechaDesde))),
                            FechaHasta = DateOnly.FromDateTime(reader.GetDateTime(nameof(Contrato.FechaHasta))),
                            Estado = reader.GetBoolean(nameof(Contrato.Estado)),
                            Inquilino = new Inquilino
                            {
                                Id = reader.GetInt32("IdInquilino"),
                                Nombre = reader.GetString("Nombre"),
                                Apellido = reader.GetString("Apellido"),
                                Dni = reader.GetString("Dni"),
                                Telefono = reader.GetString("Telefono"),
                                Email = reader.GetString("Email")
                            },
                            Inmueble = new Inmueble
                            {
                                Id = reader.GetInt32("IdInmueble"),
                                Tipo = new Tipo
                                {
                                    Id = reader.GetInt32("IdTipo"),
                                    Descripcion = reader.GetString("Descripcion"),
                                },
                                Uso = reader.GetString("Uso"),
                                Ambientes = reader.GetInt32("Ambientes"),
                                Precio = reader.GetDecimal("Precio"),
                                Coordenadas = reader.GetString("Coordenadas"),
                                Estado = reader.GetBoolean("Estado"),
                                Direccion = reader.GetString("Direccion")
                            }
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
                string query = $@"UPDATE Contratos SET {nameof(Contrato.Estado)} = true WHERE IdContrato = @Id";

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
        public Contrato BuscarActualPorInquilino(int idInquilino) {

            Contrato res = null;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = $@"SELECT * FROM Contratos c 
                JOIN Inquilinos iq ON c.IdInquilino = iq.IdInquilino 
                JOIN Inmuebles im ON c.IdInmueble = im.IdInmueble 
                JOIN Tipos t ON im.IdTipo = im.IdTipo 
                WHERE c.IdInquilino = @IdInquilino AND c.Estado = true 
                AND CURRENT_DATE() BETWEEN c.FechaDesde AND c.FechaHasta;";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IdInquilino", idInquilino);
                    connection.Open();

                    var reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        res = new Contrato
                        {
                            Id = reader.GetInt32("IdContrato"),
                            IdInquilino = reader.GetInt32(nameof(Contrato.IdInquilino)),
                            IdInmueble = reader.GetInt32(nameof(Contrato.IdInmueble)),
                            Monto = reader.GetDecimal(nameof(Contrato.Monto)),
                            FechaDesde = DateOnly.FromDateTime(reader.GetDateTime(nameof(Contrato.FechaDesde))),
                            FechaHasta = DateOnly.FromDateTime(reader.GetDateTime(nameof(Contrato.FechaHasta))),
                            Estado = reader.GetBoolean(nameof(Contrato.Estado)),
                            Inquilino = new Inquilino
                            {
                                Id = reader.GetInt32("IdInquilino"),
                                Nombre = reader.GetString("Nombre"),
                                Apellido = reader.GetString("Apellido"),
                                Dni = reader.GetString("Dni"),
                                Telefono = reader.GetString("Telefono"),
                                Email = reader.GetString("Email")
                            },
                            Inmueble = new Inmueble
                            {
                                Id = reader.GetInt32("IdInmueble"),
                                Tipo = new Tipo
                                {
                                    Id = reader.GetInt32("IdTipo"),
                                    Descripcion = reader.GetString("Descripcion"),
                                },
                                Uso = reader.GetString("Uso"),
                                Ambientes = reader.GetInt32("Ambientes"),
                                Precio = reader.GetDecimal("Precio"),
                                Coordenadas = reader.GetString("Coordenadas"),
                                Estado = reader.GetBoolean("Estado")
                            }
                        };
                    }
                }
            }
            return res;
        }
    }
}