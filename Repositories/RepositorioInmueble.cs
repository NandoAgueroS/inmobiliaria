using System.Collections;
using inmobiliaria.Models;
using MySql.Data.MySqlClient;

namespace inmobiliaria.Repositories
{
    public class RepositorioInmueble : RepositorioBase, IRepositorioInmueble
    {
        public RepositorioInmueble(IConfiguration configuration) : base(configuration)
        {

        }

        public int Alta(Inmueble m)
        {
            int res = -1;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = $@"INSERT INTO Inmuebles (
            {nameof(Inmueble.IdPropietario)},
            {nameof(Inmueble.IdTipo)},
            {nameof(Inmueble.Uso)},
            {nameof(Inmueble.Ambientes)},
            {nameof(Inmueble.Direccion)},
            {nameof(Inmueble.Precio)},
            {nameof(Inmueble.Disponible)},
            {nameof(Inmueble.Coordenadas)},
            {nameof(Inmueble.Estado)}) VALUES (
            @IdPropietario, @IdTipo, @Uso, @Ambientes, @Direccion, @Precio, @Disponible, @Coordenadas, true);
            SELECT LAST_INSERT_ID();";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IdPropietario", m.IdPropietario);
                    command.Parameters.AddWithValue("@IdTipo", m.IdTipo);
                    command.Parameters.AddWithValue("@Uso", m.Uso);
                    command.Parameters.AddWithValue("@Ambientes", m.Ambientes);
                    command.Parameters.AddWithValue("@Direccion", m.Direccion);
                    command.Parameters.AddWithValue("@Precio", m.Precio);
                    command.Parameters.AddWithValue("@Disponible", m.Disponible);
                    command.Parameters.AddWithValue("@Coordenadas", m.Coordenadas);
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
                string query = $@"UPDATE Inmuebles SET {nameof(Inmueble.Estado)} = false WHERE IdInmueble = @Id";

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

        public Inmueble BuscarPorId(int id)
        {
            Inmueble res = null;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = $@"SELECT * FROM Inmuebles i JOIN Propietarios p ON i.IdPropietario = p.IdPropietario JOIN Tipos t ON i.IdTipo = t.IdTipo WHERE i.IdInmueble = @Id AND i.Estado = true";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    connection.Open();

                    var reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        res = new Inmueble
                        {
                            Id = reader.GetInt32("IdInmueble"),
                            IdPropietario = reader.GetInt32(nameof(Inmueble.IdPropietario)),
                            IdTipo = reader.GetInt32(nameof(Inmueble.IdTipo)),
                            Uso = reader.GetString(nameof(Inmueble.Uso)),
                            Ambientes = reader.GetInt32(nameof(Inmueble.Ambientes)),
                            Direccion = reader.GetString(nameof(Inmueble.Direccion)),
                            Precio = reader.GetDecimal(nameof(Inmueble.Precio)),
                            Coordenadas = reader.GetString(nameof(Inmueble.Coordenadas)),
                            Disponible = reader.GetBoolean(nameof(Inmueble.Disponible)),
                            Estado = reader.GetBoolean(nameof(Contrato.Estado)),
                            Propietario = new Propietario
                            {
                                Id = reader.GetInt32("IdPropietario"),
                                Nombre = reader.GetString(nameof(Propietario.Nombre)),
                                Apellido = reader.GetString(nameof(Propietario.Apellido)),
                                Dni = reader.GetString(nameof(Propietario.Dni)),
                                Telefono = reader.GetString(nameof(Propietario.Telefono)),
                                Direccion = reader.GetString(nameof(Propietario.Direccion))
                            },

                            Tipo = new Tipo
                            {
                                Id = reader.GetInt32("IdTipo"),
                                Descripcion = reader.GetString(nameof(Tipo.Descripcion))
                            }

                        };
                    }
                    connection.Close();
                }

            }
            return res;
        }

        public IList<Inmueble> ListarTodos()
        {
            IList<Inmueble> res = new List<Inmueble>();
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = $@"SELECT * FROM Inmuebles i JOIN Propietarios p ON i.IdPropietario = p.IdPropietario JOIN Tipos t ON i.IdTipo = t.IdTipo WHERE i.Estado = true;";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        res.Add(new Inmueble
                        {
                            Id = reader.GetInt32("IdInmueble"),
                            IdPropietario = reader.GetInt32(nameof(Inmueble.IdPropietario)),
                            IdTipo = reader.GetInt32(nameof(Inmueble.IdTipo)),
                            Uso = reader.GetString(nameof(Inmueble.Uso)),
                            Ambientes = reader.GetInt32(nameof(Inmueble.Ambientes)),
                            Direccion = reader.GetString(nameof(Inmueble.Direccion)),
                            Precio = reader.GetDecimal(nameof(Inmueble.Precio)),
                            Coordenadas = reader.GetString(nameof(Inmueble.Coordenadas)),
                            Estado = reader.GetBoolean(nameof(Contrato.Estado)),
                            Propietario = new Propietario
                            {
                                Id = reader.GetInt32("IdPropietario"),
                                Nombre = reader.GetString(nameof(Propietario.Nombre)),
                                Apellido = reader.GetString(nameof(Propietario.Apellido)),
                                Dni = reader.GetString(nameof(Propietario.Dni)),
                                Telefono = reader.GetString(nameof(Propietario.Telefono)),
                                Direccion = reader.GetString(nameof(Propietario.Direccion))
                            },

                            Tipo = new Tipo
                            {
                                Id = reader.GetInt32("IdTipo"),
                                Descripcion = reader.GetString(nameof(Tipo.Descripcion))
                            }
                        });
                    }
                    connection.Close();
                }
            }
            return res;
        }

        public int Modificacion(Inmueble m)
        {
            int res = -1;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = $@"UPDATE Inmuebles SET
                {nameof(Inmueble.IdPropietario)} = @IdPropietario,
                {nameof(Inmueble.IdTipo)} = @IdTipo,
                {nameof(Inmueble.Uso)} = @uso,
                {nameof(Inmueble.Ambientes)} = @Ambientes,
                {nameof(Inmueble.Direccion)} = @Direccion,
                {nameof(Inmueble.Disponible)} = @Disponible,
                {nameof(Inmueble.Precio)} = @Precio,
                {nameof(Inmueble.Coordenadas)} = @Coordenadas
                WHERE IdInmueble = @Id;";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", m.Id);
                    command.Parameters.AddWithValue("@IdPropietario", m.IdPropietario);
                    command.Parameters.AddWithValue("@IdTipo", m.IdTipo);
                    command.Parameters.AddWithValue("@Uso", m.Uso);
                    command.Parameters.AddWithValue("@Ambientes", m.Ambientes);
                    command.Parameters.AddWithValue("@Direccion", m.Direccion);
                    command.Parameters.AddWithValue("@Disponible", m.Disponible);
                    command.Parameters.AddWithValue("@Precio", m.Precio);
                    command.Parameters.AddWithValue("@Coordenadas", m.Coordenadas);

                    connection.Open();
                    res = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return res;
        }

        public IList<Inmueble> BuscarPorPropietario(int id)
        {
            IList<Inmueble> res = new List<Inmueble>();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {


                string query = $@"SELECT * FROM Inmuebles WHERE IdPropietario = @Id;";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        res.Add(new Inmueble
                        {
                            Id = reader.GetInt32("IdInmueble"),
                            IdPropietario = reader.GetInt32(nameof(Inmueble.IdPropietario)),
                            IdTipo = reader.GetInt32(nameof(Inmueble.IdTipo)),
                            Uso = reader.GetString(nameof(Inmueble.Uso)),
                            Ambientes = reader.GetInt32(nameof(Inmueble.Ambientes)),
                            Direccion = reader.GetString(nameof(Inmueble.Direccion)),
                            Precio = reader.GetDecimal(nameof(Inmueble.Precio)),
                            Coordenadas = reader.GetString(nameof(Inmueble.Coordenadas)),
                            Estado = reader.GetBoolean(nameof(Inmueble.Estado))
                        });
                    }
                    connection.Close();
                }

            }
            return res;
        }
        public IList<Inmueble> BuscarPorDireccion(string direccion)
        {
            IList<Inmueble> res = new List<Inmueble>();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = $@"SELECT * FROM Inmuebles WHERE direccion LIKE @Direccion;";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Direccion", $"%{direccion}%");
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        res.Add(new Inmueble
                        {
                            Id = reader.GetInt32("IdInmueble"),
                            IdPropietario = reader.GetInt32(nameof(Inmueble.IdPropietario)),
                            IdTipo = reader.GetInt32(nameof(Inmueble.IdTipo)),
                            Uso = reader.GetString(nameof(Inmueble.Uso)),
                            Ambientes = reader.GetInt32(nameof(Inmueble.Ambientes)),
                            Direccion = reader.GetString(nameof(Inmueble.Direccion)),
                            Precio = reader.GetDecimal(nameof(Inmueble.Precio)),
                            Coordenadas = reader.GetString(nameof(Inmueble.Coordenadas)),
                            Estado = reader.GetBoolean(nameof(Inmueble.Estado))
                        });

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
                string query = $@"UPDATE Inmuebles SET {nameof(Inmueble.Estado)} = true WHERE IdInmueble = @Id";

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

        public bool VerificarDesocupado(DateOnly fechaDesde, DateOnly fechaHasta, int idInmueble, int? idContrato)
        {
            bool res = false;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = $@"
                SELECT i.IdInmueble
                FROM Inmuebles i
                LEFT JOIN Contratos c ON i.IdInmueble = c.IdInmueble
                    AND c.Estado = 1
                    AND c.FechaDesde < @FechaHasta
                    AND c.FechaHasta > @FechaDesde
                    AND (@IdContrato IS NULL OR c.IdContrato != @IdContrato)
                WHERE i.IdInmueble = @IdInmueble
                GROUP BY i.IdInmueble
                HAVING COUNT(c.IdContrato) = 0;
                ";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    connection.Open();
                    command.Parameters.AddWithValue("@FechaDesde", fechaDesde.ToString("yyyy-MM-dd"));
                    command.Parameters.AddWithValue("@FechaHasta", fechaHasta.ToString("yyyy-MM-dd"));
                    command.Parameters.AddWithValue("@IdInmueble", idInmueble);
                    command.Parameters.AddWithValue("@IdContrato", idContrato);

                    var reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        res = true;
                    }
                }
                connection.Close();
            }
            return res;
        }

        public bool VerificarDesocupado(DateOnly fechaDesde, DateOnly fechaHasta, int id)
        {
            bool res = false;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = $@"
                SELECT i.*, t.*, p.*
                FROM Inmuebles i
                JOIN Tipos t ON i.IdTipo = t.IdTipo
                JOIN Propietarios p ON i.IdPropietario = p.IdPropietario
                LEFT JOIN Contratos c
                ON c.IdInmueble = i.IdInmueble
                AND c.Estado = 1
                AND c.FechaDesde <= @FechaHasta
                AND c.FechaHasta  >= @FechaDesde
                WHERE i.Estado = 1
                AND c.IdContrato IS NULL
                AND i.IdInmueble = @Id;
                ";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    connection.Open();
                    command.Parameters.AddWithValue("@FechaDesde", fechaDesde.ToString("yyyy-MM-dd"));
                    command.Parameters.AddWithValue("@FechaHasta", fechaHasta.ToString("yyyy-MM-dd"));
                    command.Parameters.AddWithValue("@Id", id);
                    var reader = command.ExecuteReader();
                    if (reader.Read())
                        res = true;
                }
                connection.Close();
            }
            return res;

        }
        public bool VerificarDisponible(int id)
        {
            bool res = false;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = $@"
                SELECT * 
                FROM Inmuebles i
                WHERE i.Estado = 1
                AND i.Disponible = 1
                AND i.IdInmueble = @Id;
                ";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    connection.Open();
                    command.Parameters.AddWithValue("@Id", id);
                    var reader = command.ExecuteReader();
                    if (reader.Read())
                        res = true;
                }
                connection.Close();
            }
            return res;

        }
        public IList<Inmueble> ListarPorDisponible(bool disponibles)
        {
            IList<Inmueble> res = new List<Inmueble>();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = $@"SELECT * FROM Inmuebles WHERE Estado = true && Disponible = @Disponible;";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("Disponible", disponibles);
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        res.Add(new Inmueble
                        {
                            Id = reader.GetInt32("IdInmueble"),
                            IdPropietario = reader.GetInt32(nameof(Inmueble.IdPropietario)),
                            IdTipo = reader.GetInt32(nameof(Inmueble.IdTipo)),
                            Uso = reader.GetString(nameof(Inmueble.Uso)),
                            Ambientes = reader.GetInt32(nameof(Inmueble.Ambientes)),
                            Direccion = reader.GetString(nameof(Inmueble.Direccion)),
                            Precio = reader.GetDecimal(nameof(Inmueble.Precio)),
                            Coordenadas = reader.GetString(nameof(Inmueble.Coordenadas)),
                            Estado = reader.GetBoolean(nameof(Inmueble.Estado))
                        });
                    }
                    connection.Close();
                }

            }
            return res;

        }

        public IList<Inmueble> ListarDesocupados(DateOnly fechaDesde, DateOnly fechaHasta)
        {
            IList<Inmueble> res = new List<Inmueble>();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = $@"
                SELECT i.*, t.*, p.*
                FROM Inmuebles i
                JOIN Tipos t ON i.IdTipo = t.IdTipo
                JOIN Propietarios p ON i.IdPropietario = p.IdPropietario
                LEFT JOIN Contratos c
                ON c.IdInmueble = i.IdInmueble
                AND c.Estado = 1
                AND i.Disponible = true 
                AND c.FechaDesde <= @FechaHasta
                AND c.FechaHasta  >= @FechaDesde
                WHERE i.Estado = 1
                AND c.IdContrato IS NULL;
                ;
                ";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    connection.Open();
                    command.Parameters.AddWithValue("@FechaDesde", fechaDesde.ToString("yyyy-MM-dd"));
                    command.Parameters.AddWithValue("@FechaHasta", fechaHasta.ToString("yyyy-MM-dd"));
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        res.Add(new Inmueble
                        {
                            Id = reader.GetInt32("IdInmueble"),
                            IdPropietario = reader.GetInt32(nameof(Inmueble.IdPropietario)),
                            IdTipo = reader.GetInt32(nameof(Inmueble.IdTipo)),
                            Uso = reader.GetString(nameof(Inmueble.Uso)),
                            Ambientes = reader.GetInt32(nameof(Inmueble.Ambientes)),
                            Direccion = reader.GetString(nameof(Inmueble.Direccion)),
                            Precio = reader.GetDecimal(nameof(Inmueble.Precio)),
                            Coordenadas = reader.GetString(nameof(Inmueble.Coordenadas)),
                            Estado = reader.GetBoolean(nameof(Inmueble.Estado)),
                            Propietario = new Propietario
                            {
                                Id = reader.GetInt32("IdPropietario"),
                                Nombre = reader.GetString(nameof(Propietario.Nombre)),
                                Apellido = reader.GetString(nameof(Propietario.Apellido)),
                                Dni = reader.GetString(nameof(Propietario.Dni)),
                                Telefono = reader.GetString(nameof(Propietario.Telefono)),
                                Email = reader.GetString(nameof(Propietario.Email)),
                                Direccion = reader.GetString(nameof(Propietario.Direccion))
                            },

                            Tipo = new Tipo
                            {
                                Id = reader.GetInt32("IdTipo"),
                                Descripcion = reader.GetString(nameof(Tipo.Descripcion)),
                            }
                        });
                    }
                    connection.Close();
                }

            }
            return res;
        }

        public IList<Inmueble> ListarDisponiblesYDesocupados(DateOnly fechaDesde, DateOnly fechaHasta)
        {
            IList<Inmueble> res = new List<Inmueble>();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = $@"
                SELECT i.*, t.*, p.*
                FROM Inmuebles i
                JOIN Tipos t ON i.IdTipo = t.IdTipo
                JOIN Propietarios p ON i.IdPropietario = p.IdPropietario
                LEFT JOIN Contratos c
                ON c.IdInmueble = i.IdInmueble
                AND c.Estado = 1
                AND i.Disponible = true 
                AND c.FechaDesde <= @FechaHasta
                AND c.FechaHasta  >= @FechaDesde
                WHERE i.Estado = 1
                AND i.Disponible = true
                AND c.IdContrato IS NULL;
                ;
                ";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    connection.Open();
                    command.Parameters.AddWithValue("@FechaDesde", fechaDesde.ToString("yyyy-MM-dd"));
                    command.Parameters.AddWithValue("@FechaHasta", fechaHasta.ToString("yyyy-MM-dd"));
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        res.Add(new Inmueble
                        {
                            Id = reader.GetInt32("IdInmueble"),
                            IdPropietario = reader.GetInt32(nameof(Inmueble.IdPropietario)),
                            IdTipo = reader.GetInt32(nameof(Inmueble.IdTipo)),
                            Uso = reader.GetString(nameof(Inmueble.Uso)),
                            Ambientes = reader.GetInt32(nameof(Inmueble.Ambientes)),
                            Direccion = reader.GetString(nameof(Inmueble.Direccion)),
                            Precio = reader.GetDecimal(nameof(Inmueble.Precio)),
                            Coordenadas = reader.GetString(nameof(Inmueble.Coordenadas)),
                            Estado = reader.GetBoolean(nameof(Inmueble.Estado)),
                            Propietario = new Propietario
                            {
                                Id = reader.GetInt32("IdPropietario"),
                                Nombre = reader.GetString(nameof(Propietario.Nombre)),
                                Apellido = reader.GetString(nameof(Propietario.Apellido)),
                                Dni = reader.GetString(nameof(Propietario.Dni)),
                                Telefono = reader.GetString(nameof(Propietario.Telefono)),
                                Email = reader.GetString(nameof(Propietario.Email)),
                                Direccion = reader.GetString(nameof(Propietario.Direccion))
                            },

                            Tipo = new Tipo
                            {
                                Id = reader.GetInt32("IdTipo"),
                                Descripcion = reader.GetString(nameof(Tipo.Descripcion)),
                            }
                        });
                    }
                    connection.Close();
                }

            }
            return res;
        }
    }


}
