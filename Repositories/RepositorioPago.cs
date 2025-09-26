using inmobiliaria.Models;
using MySql.Data.MySqlClient;

namespace inmobiliaria.Repositories
{
    public class RepositorioPago : RepositorioBase, IRepositorioPago
    {
        public RepositorioPago(IConfiguration configuration) : base(configuration)
        {
        }

        public int Alta(Pago m)
        {
            int res = -1;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = $@"INSERT INTO Pagos (
                {nameof(Pago.NumeroPago)},
                {nameof(Pago.Concepto)},
                {nameof(Pago.Monto)},
                {nameof(Pago.Fecha)},
                {nameof(Pago.CorrespondeAMes)},
                {nameof(Pago.IdContrato)},
                {nameof(Pago.CreadoPor)},
                {nameof(Pago.Estado)}) VALUES (
                @NumeroPago, @Concepto, @Monto, @Fecha, @CorrespondeAMes, @IdContrato, @CreadoPor, true);
                SELECT LAST_INSERT_ID();";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@NumeroPago", m.NumeroPago);
                    command.Parameters.AddWithValue("@Concepto", m.Concepto);
                    command.Parameters.AddWithValue("@Monto", m.Monto);
                    command.Parameters.AddWithValue("@Fecha", m.Fecha.ToString("yyyy-MM-dd"));
                    command.Parameters.AddWithValue("@CorrespondeAMes", m.CorrespondeAMes == null ? DBNull.Value : m.CorrespondeAMes.Value.ToString("yyyy-MM-dd"));
                    command.Parameters.AddWithValue("@IdContrato", m.IdContrato);
                    command.Parameters.AddWithValue("@CreadoPor", m.CreadoPor);
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
            throw new NotImplementedException();
        }

        public int Baja(int id, int anuladoPor)
        {
            int res = -1;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = $@"UPDATE Pagos SET {nameof(Pago.Estado)}= false, {nameof(Pago.AnuladoPor)} = @AnuladoPor WHERE IdPago = @Id";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    command.Parameters.AddWithValue("@AnuladoPor", anuladoPor);
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return res;
        }

        public IList<Pago> BuscarPagoDeInquilino(int idInquilino)
        {
            IList<Pago> res = new List<Pago>();


            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = $@"SELECT * FROM Pagos p JOIN Contratos c ON p.IdContrato = c.IdContrato JOIN Inquilinos i ON c.IdInquilino = i.IdInquilino AND p.Estado = true";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IdInquilino", idInquilino);

                    connection.Open();

                    var reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        res.Add(new Pago
                        {
                            Id = reader.GetInt32("IdPago"),
                            NumeroPago = reader.GetString(nameof(Pago.NumeroPago)),
                            Concepto = reader.GetString(nameof(Pago.Concepto)),
                            Monto = reader.GetDecimal(nameof(Pago.Monto)),
                            Fecha = DateOnly.FromDateTime(reader.GetDateTime(nameof(Pago.Fecha))),
                            IdContrato = reader.GetInt32(nameof(Pago.IdContrato)),
                            Estado = reader.GetBoolean(nameof(Pago.Estado)),


                            Contrato = new Contrato
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
                                    Nombre = reader.GetString(nameof(Inquilino.Nombre)),
                                    Apellido = reader.GetString(nameof(Inquilino.Apellido)),
                                    Dni = reader.GetString(nameof(Inquilino.Dni)),
                                    Telefono = reader.GetString(nameof(Inquilino.Telefono))
                                }

                            }

                        });
                    }
                    connection.Close();
                }

            }
            return res;
        }

        public IList<Pago> BuscarPagoPorContrato(int idContrato)
        {

            IList<Pago> res = new List<Pago>();
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = $@"SELECT * FROM Pagos p JOIN Contratos c ON p.IdContrato = c.IdContrato AND c.Estado = true";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IdContrato", idContrato);

                    connection.Open();

                    var reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        res.Add(new Pago
                        {
                            Id = reader.GetInt32("IdPago"),
                            NumeroPago = reader.GetString(nameof(Pago.NumeroPago)),
                            Concepto = reader.GetString(nameof(Pago.Concepto)),
                            Monto = reader.GetDecimal(nameof(Pago.Monto)),
                            Fecha = DateOnly.FromDateTime(reader.GetDateTime(nameof(Pago.Fecha))),
                            IdContrato = reader.GetInt32(nameof(Pago.IdContrato)),
                            Estado = reader.GetBoolean(nameof(Pago.Estado)),

                            Contrato = new Contrato
                            {
                                Id = reader.GetInt32("IdContrato"),
                                IdInquilino = reader.GetInt32(nameof(Contrato.IdInquilino)),
                                IdInmueble = reader.GetInt32(nameof(Contrato.IdInmueble)),
                                Monto = reader.GetDecimal(nameof(Contrato.Monto)),
                                FechaDesde = DateOnly.FromDateTime(reader.GetDateTime(nameof(Contrato.FechaDesde))),
                                FechaHasta = DateOnly.FromDateTime(reader.GetDateTime(nameof(Contrato.FechaHasta))),
                                Estado = reader.GetBoolean(nameof(Contrato.Estado))
                            }

                        });
                    }
                    connection.Close();
                }

            }
            return res;

        }

        public IList<Pago> ListarPagosPorContratoconFechaActual(int idContrato)
        {

            IList<Pago> res = new List<Pago>();
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = $@"SELECT * FROM Pagos p JOIN Contratos c ON p.IdContrato = c.IdContrato WHERE CURDATE() BETWEEN c.FechaDesde AND c.FechaHasta  AND c.IdContrato = @IdContrato AND c.Estado = true";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IdContrato", idContrato);

                    connection.Open();

                    var reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        res.Add(new Pago
                        {
                            Id = reader.GetInt32("IdPago"),
                            NumeroPago = reader.GetString(nameof(Pago.NumeroPago)),
                            Concepto = reader.GetString(nameof(Pago.Concepto)),
                            Monto = reader.GetDecimal(nameof(Pago.Monto)),
                            Fecha = DateOnly.FromDateTime(reader.GetDateTime(nameof(Pago.Fecha))),
                            IdContrato = reader.GetInt32(nameof(Pago.IdContrato)),
                            Estado = reader.GetBoolean(nameof(Pago.Estado)),

                            Contrato = new Contrato
                            {
                                Id = reader.GetInt32("IdContrato"),
                                IdInquilino = reader.GetInt32(nameof(Contrato.IdInquilino)),
                                IdInmueble = reader.GetInt32(nameof(Contrato.IdInmueble)),
                                Monto = reader.GetDecimal(nameof(Contrato.Monto)),
                                FechaDesde = DateOnly.FromDateTime(reader.GetDateTime(nameof(Contrato.FechaDesde))),
                                FechaHasta = DateOnly.FromDateTime(reader.GetDateTime(nameof(Contrato.FechaHasta))),
                                Estado = reader.GetBoolean(nameof(Contrato.Estado))
                            }

                        });
                    }
                    connection.Close();

                }

            }
            return res;




        }

        public Pago BuscarPorId(int id)
        {
            Pago res = null;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = $@"SELECT * FROM Pagos p WHERE p.idPagos = @id AND estado = true";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    connection.Open();

                    var reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        res = new Pago
                        {
                            Id = reader.GetInt32("IdPago"),
                            NumeroPago = reader.GetString(nameof(Pago.NumeroPago)),
                            Concepto = reader.GetString(nameof(Pago.Concepto)),
                            Monto = reader.GetDecimal(nameof(Pago.Monto)),
                            Fecha = DateOnly.FromDateTime(reader.GetDateTime(nameof(Pago.Fecha))),
                            IdContrato = reader.GetInt32(nameof(Pago.IdContrato)),
                            Estado = reader.GetBoolean(nameof(Pago.Estado)),

                            Contrato = new Contrato
                            {
                                Id = reader.GetInt32("IdContrato"),
                                IdInquilino = reader.GetInt32(nameof(Contrato.IdInquilino)),
                                IdInmueble = reader.GetInt32(nameof(Contrato.IdInmueble)),
                                Monto = reader.GetDecimal(nameof(Contrato.Monto)),
                                FechaDesde = DateOnly.FromDateTime(reader.GetDateTime(nameof(Contrato.FechaDesde))),
                                FechaHasta = DateOnly.FromDateTime(reader.GetDateTime(nameof(Contrato.FechaHasta))),
                                Estado = reader.GetBoolean(nameof(Contrato.Estado))
                            }

                        };
                    }
                    connection.Close();
                }

            }
            return res;
        }

        public IList<Pago> ListarTodos()
        {
            IList<Pago> res = new List<Pago>();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = $@"SELECT * FROM Pagos p JOIN Contratos c ON p.IdContrato = c.IdContrato";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {

                    connection.Open();

                    var reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        res.Add(new Pago
                        {
                            Id = reader.GetInt32("IdPago"),
                            NumeroPago = reader.GetString(nameof(Pago.NumeroPago)),
                            Concepto = reader.GetString(nameof(Pago.Concepto)),
                            Monto = reader.GetDecimal(nameof(Pago.Monto)),
                            Fecha = DateOnly.FromDateTime(reader.GetDateTime(nameof(Pago.Fecha))),
                            IdContrato = reader.GetInt32(nameof(Pago.IdContrato)),
                            CorrespondeAMes = reader.IsDBNull(reader.GetOrdinal(nameof(Pago.CorrespondeAMes))) ? null : DateOnly.FromDateTime(reader.GetDateTime(nameof(Pago.CorrespondeAMes))),
                            Estado = reader.GetBoolean(nameof(Pago.Estado)),
                            CreadoPor = reader.GetInt32(nameof(Pago.CreadoPor)),
                            AnuladoPor = reader.IsDBNull(reader.GetOrdinal(nameof(Pago.AnuladoPor))) ? null : reader.GetInt32(nameof(Pago.AnuladoPor)),

                            Contrato = new Contrato
                            {
                                Id = reader.GetInt32("IdContrato"),
                                IdInquilino = reader.GetInt32(nameof(Contrato.IdInquilino)),
                                IdInmueble = reader.GetInt32(nameof(Contrato.IdInmueble)),
                                Monto = reader.GetDecimal(nameof(Contrato.Monto)),
                                FechaDesde = DateOnly.FromDateTime(reader.GetDateTime(nameof(Contrato.FechaDesde))),
                                FechaHasta = DateOnly.FromDateTime(reader.GetDateTime(nameof(Contrato.FechaHasta))),
                            }

                        });
                    }
                    connection.Close();
                }

            }
            return res;

        }

        public int Modificacion(Pago m)
        {
            int res = -1;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = $@"UPDATE Pagos SET
                 {nameof(Pago.NumeroPago)} = @NumeroPago,
                {nameof(Pago.Concepto)} = @Concepto,
                {nameof(Pago.Monto)} = @Monto,
                {nameof(Pago.Fecha)} = @Fecha,
                {nameof(Pago.IdContrato)} = @IdContrato
                WHERE IdPago = @Id;";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", m.Id);
                    command.Parameters.AddWithValue("@NumeroPago", m.NumeroPago);
                    command.Parameters.AddWithValue("@Concepto", m.Concepto);
                    command.Parameters.AddWithValue("@Monto", m.Monto);
                    command.Parameters.AddWithValue("@Fecha", m.Fecha.ToString("yyyy-MM-dd"));
                    command.Parameters.AddWithValue("@IdContrato", m.IdContrato);
                    connection.Open();
                    res = command.ExecuteNonQuery();
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
                string query = $@"UPDATE Pagos SET {nameof(Pago.Estado)} = true WHERE IdPago = @Id";

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

        public IList<Pago> BuscarPagoPorFecha(DateOnly fechaAcorroborar)
        {
            IList<Pago> res = new List<Pago>();
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = $@"SELECT * FROM Pagos p JOIN Contratos c ON p.IdContrato = c.IdContrato WHERE MONTH(p.Fecha) = MONTH(@FechaAcorroborar) AND @FechaAcorroborar BETWEEN c.FechaDesde AND c.FechaHasta AND c.Estado = true";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@FechaAcorroborar", fechaAcorroborar);

                    connection.Open();

                    var reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        res.Add(new Pago
                        {
                            Id = reader.GetInt32("IdPago"),
                            NumeroPago = reader.GetString(nameof(Pago.NumeroPago)),
                            Concepto = reader.GetString(nameof(Pago.Concepto)),
                            Monto = reader.GetDecimal(nameof(Pago.Monto)),
                            Fecha = DateOnly.FromDateTime(reader.GetDateTime(nameof(Pago.Fecha))),
                            IdContrato = reader.GetInt32(nameof(Pago.IdContrato)),
                            CorrespondeAMes = DateOnly.FromDateTime(reader.GetDateTime(nameof(Pago.CorrespondeAMes))),
                            Estado = reader.GetBoolean(nameof(Pago.Estado)),

                            Contrato = new Contrato
                            {
                                Id = reader.GetInt32("IdContrato"),
                                IdInquilino = reader.GetInt32(nameof(Contrato.IdInquilino)),
                                IdInmueble = reader.GetInt32(nameof(Contrato.IdInmueble)),
                                Monto = reader.GetDecimal(nameof(Contrato.Monto)),
                                FechaDesde = DateOnly.FromDateTime(reader.GetDateTime(nameof(Contrato.FechaDesde))),
                                FechaHasta = DateOnly.FromDateTime(reader.GetDateTime(nameof(Contrato.FechaHasta))),
                                Estado = reader.GetBoolean(nameof(Contrato.Estado))
                            }

                        });
                    }
                    connection.Close();

                }

            }
            return res;

        }
        public Pago BuscarUltimoPago(int idContrato)
        {
            Pago res = null;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = $@"SELECT * FROM Pagos p JOIN Contratos c ON p.IdContrato = c.IdContrato 
                WHERE c.Estado = true
                AND c.IdContrato = @IdContrato
                AND p.CorrespondeAMes IS NOT NULL
                ORDER BY p.CorrespondeAMes DESC
                LIMIT 1";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {

                    command.Parameters.AddWithValue("@IdContrato", idContrato);
                    connection.Open();

                    var reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        res = new Pago
                        {
                            Id = reader.GetInt32("IdPago"),
                            NumeroPago = reader.GetString(nameof(Pago.NumeroPago)),
                            Concepto = reader.GetString(nameof(Pago.Concepto)),
                            Monto = reader.GetDecimal(nameof(Pago.Monto)),
                            Fecha = DateOnly.FromDateTime(reader.GetDateTime(nameof(Pago.Fecha))),
                            CorrespondeAMes = DateOnly.FromDateTime(reader.GetDateTime(nameof(Pago.CorrespondeAMes))),
                            IdContrato = reader.GetInt32(nameof(Pago.IdContrato)),
                            Estado = reader.GetBoolean(nameof(Pago.Estado)),

                            Contrato = new Contrato
                            {
                                Id = reader.GetInt32("IdContrato"),
                                IdInquilino = reader.GetInt32(nameof(Contrato.IdInquilino)),
                                IdInmueble = reader.GetInt32(nameof(Contrato.IdInmueble)),
                                Monto = reader.GetDecimal(nameof(Contrato.Monto)),
                                FechaDesde = DateOnly.FromDateTime(reader.GetDateTime(nameof(Contrato.FechaDesde))),
                                FechaHasta = DateOnly.FromDateTime(reader.GetDateTime(nameof(Contrato.FechaHasta))),
                                Estado = reader.GetBoolean(nameof(Contrato.Estado))
                            }


                        };
                        connection.Close();

                    }
                }

                return res;
            }
        }

        public IList<Pago> ListarPorInquilino(int idInquilino)
        {

            IList<Pago> res = new List<Pago>();
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = $@"SELECT * FROM Pagos p JOIN Contratos c ON p.IdContrato = c.IdContrato 
                WHERE c.IdInquilino = @IdInquilino";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {

                    command.Parameters.AddWithValue("@IdInquilino", idInquilino);
                    connection.Open();

                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        res.Add(new Pago
                        {
                            Id = reader.GetInt32("IdPago"),
                            NumeroPago = reader.GetString(nameof(Pago.NumeroPago)),
                            Concepto = reader.GetString(nameof(Pago.Concepto)),
                            Monto = reader.GetDecimal(nameof(Pago.Monto)),
                            Fecha = DateOnly.FromDateTime(reader.GetDateTime(nameof(Pago.Fecha))),
                            CorrespondeAMes = reader[nameof(Pago.CorrespondeAMes)] is DBNull ? null : (DateOnly?)DateOnly.FromDateTime(reader.GetDateTime(nameof(Pago.CorrespondeAMes))),
                            IdContrato = reader.GetInt32(nameof(Pago.IdContrato)),
                            Estado = reader.GetBoolean(nameof(Pago.Estado)),

                            Contrato = new Contrato
                            {
                                Id = reader.GetInt32("IdContrato"),
                                IdInquilino = reader.GetInt32(nameof(Contrato.IdInquilino)),
                                IdInmueble = reader.GetInt32(nameof(Contrato.IdInmueble)),
                                Monto = reader.GetDecimal(nameof(Contrato.Monto)),
                                FechaDesde = DateOnly.FromDateTime(reader.GetDateTime(nameof(Contrato.FechaDesde))),
                                FechaHasta = DateOnly.FromDateTime(reader.GetDateTime(nameof(Contrato.FechaHasta))),
                                Estado = reader.GetBoolean(nameof(Contrato.Estado))
                            }


                        });
                    }
                    connection.Close();

                }

                return res;
            }
        }
        public int ObtenerUltimoNumeroPago(int idContrato)
        {
            int res = -1;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = $@"SELECT NumeroPago FROM `Pagos` WHERE IdContrato = @IdContrato ORDER BY NumeroPago DESC LIMIT 1;";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IdContrato", idContrato);
                    connection.Open();
                    res = Convert.ToInt32(command.ExecuteScalar());
                    connection.Close();
                }
            }
            return res;
        }

        public int ContarPagosMensuales(int idContrato)
        {

            int res = -1;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = $@"SELECT COUNT(CorrespondeAMes) FROM Pagos WHERE IdContrato = @IdContrato AND CorrespondeAMes IS NOT NULL";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IdContrato", idContrato);
                    connection.Open();
                    res = Convert.ToInt32(command.ExecuteScalar());
                    connection.Close();
                }
            }
            return res;
        }
        public bool BuscarMulta(int idContrato)
        {
            bool res = false;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = $@"SELECT * FROM Pagos WHERE IdContrato = @IdContrato AND Concepto ='Multa';";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IdContrato", idContrato);
                    connection.Open();

                    var reader = command.ExecuteReader();
                    if (reader.Read())
                        res = true;
                    connection.Close();
                }
            }
            return res;
        }
    }
}
