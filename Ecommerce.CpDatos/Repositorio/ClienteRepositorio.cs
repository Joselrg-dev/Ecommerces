using Ecommerce.CpCommons;
using Ecommerce.CpDatos.Interfaces;
using Ecommerce.CpEntities.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.CpDatos.Repositorio
{
    public class ClienteRepositorio : IClienteRepositorio
    {
        public bool Actualizar(Clientes clientes, out string mensaje)
        {
            bool resultado;
            mensaje = string.Empty;

            try
            {
                using (var conn = DbConnectionHelper.GetConnection())
                using (var cmd = new SqlCommand("sp_actualizarCliente", conn))
                {
                    cmd.Parameters.AddWithValue("IdCliente", clientes.IdCliente);
                    cmd.Parameters.AddWithValue("NombreCliente", clientes.NombreCliente);
                    cmd.Parameters.AddWithValue("Apellido1Cliente", clientes.Apellido1Cliente);
                    cmd.Parameters.AddWithValue("Apellido2Cliente", clientes.Apellido2Cliente);
                    cmd.Parameters.AddWithValue("TelefonoCliente", clientes.TelefonoCliente);
                    cmd.Parameters.AddWithValue("CorreoCliente", clientes.CorreoCliente);
                    cmd.Parameters.AddWithValue("Estado", clientes.Estado);
                    cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    conn.Open();
                    cmd.ExecuteNonQuery();

                    resultado = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
                    mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                resultado = false;
                mensaje = ex.Message;
            }

            return resultado;
        }

        public bool CambiarContraseña(int idCliente, string nuevaContra, out string mensaje)
        {
            bool resultado;
            mensaje = string.Empty;

            try
            {
                using (var conn = DbConnectionHelper.GetConnection())
                using (var cmd = new SqlCommand("UPDATE sr.Clientes SET Clave = @nuevaContra, Reestablecer = 0 where IdCliente = @Id", conn))
                {
                    cmd.Parameters.AddWithValue("@Id", idCliente);
                    cmd.Parameters.AddWithValue("@nuevaContra", nuevaContra);
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.Text;

                    conn.Open();
                    resultado = cmd.ExecuteNonQuery() > 0 ? true : false;
                }
            }
            catch (Exception ex)
            {
                resultado = false;
                mensaje = ex.Message;
            }

            return resultado;
        }

        public bool Eliminar(int id, out string mensaje)
        {
            bool resultado;
            mensaje = string.Empty;

            try
            {
                using (var conn = DbConnectionHelper.GetConnection())
                using (var cmd = new SqlCommand("sp_eliminarCliente", conn))
                {
                    cmd.Parameters.AddWithValue("IdCliente", id);
                    cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    conn.Open();
                    cmd.ExecuteNonQuery();

                    resultado = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
                    mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                resultado = false;
                mensaje = ex.Message;
            }

            return resultado;
        }

        public int Insertar(Clientes clientes, out string mensaje)
        {
            int idAuto;
            mensaje = string.Empty;
            try
            {
                using (var conn = DbConnectionHelper.GetConnection())
                using (var cmd = new SqlCommand("sp_registrarCliente", conn))
                {
                    cmd.Parameters.AddWithValue("NombreCliente", clientes.NombreCliente);
                    cmd.Parameters.AddWithValue("Apellido1Cliente", clientes.Apellido1Cliente);
                    cmd.Parameters.AddWithValue("Apellido2Cliente", clientes.Apellido2Cliente);
                    cmd.Parameters.AddWithValue("TelefonoCliente", clientes.TelefonoCliente);
                    cmd.Parameters.AddWithValue("CorreoCliente", clientes.CorreoCliente);
                    cmd.Parameters.AddWithValue("ClaveCliente", clientes.Clave);
                    cmd.Parameters.AddWithValue("Estado", clientes.Estado);
                    cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    conn.Open();
                    cmd.ExecuteNonQuery();

                    idAuto = Convert.ToInt32(cmd.Parameters["Resultado"].Value);
                    mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                idAuto = 0;
                mensaje = ex.Message;
            }

            return idAuto;
        }

        public Clientes ObtenerPorId(int id)
        {
            throw new NotImplementedException();
        }

        public List<Clientes> ObtenerTodo()
        {
            var listCliente = new List<Clientes>();

            try
            {
                using (SqlConnection conn = DbConnectionHelper.GetConnection())
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("SELECT cl.IdCliente, cl.NombreCliente,");
                    sb.AppendLine("cl.Apellido1Cliente, cl.Apellido2Cliente,");
                    sb.AppendLine("cl.TelefonoCliente,");
                    sb.AppendLine("cl.CorreoCliente, cl.Clave, cl.Estado, cl.Reestablecer,");
                    sb.AppendLine("FROM sr.Clientes cl");

                    SqlCommand cmd = new SqlCommand(sb.ToString(), conn)
                    {
                        CommandType = CommandType.Text
                    };
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var Cliente = new Clientes
                            {
                                IdCliente = Convert.ToInt32(reader["IdCliente"]),
                                NombreCliente = reader["NombreCliente"].ToString(),
                                Apellido1Cliente = reader["Apellido1Cliente"].ToString(),
                                Apellido2Cliente = reader["Apellido2Cliente"].ToString(),
                                TelefonoCliente = reader["TelefonoCliente"].ToString(),
                                CorreoCliente = reader["CorreoCliente"].ToString(),
                                Clave = reader["Clave"].ToString(),
                                Estado = Convert.ToBoolean(reader["Estado"]),
                                Reestablecer = reader["Reestablecer"] == DBNull.Value
                                               ? (bool?)null
                                               : Convert.ToBoolean(reader["Reestablecer"]),
                            };
                            listCliente.Add(Cliente);
                        }
                    }
                }
            }
            catch
            {
                listCliente = new List<Clientes>();
            }

            return listCliente;
        }

        public bool ReestablecerContraseña(int idCliente, string nuevaContra, out string mensaje)
        {
            bool resultado;
            mensaje = string.Empty;

            try
            {
                using (var conn = DbConnectionHelper.GetConnection())
                using (var cmd = new SqlCommand("UPDATE sr.Clientes SET Clave = @clave, Reestablecer = 1 where IdCliente = @Id", conn))
                {
                    cmd.Parameters.AddWithValue("@Id", idCliente);
                    cmd.Parameters.AddWithValue("@clave", nuevaContra);
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.Text;

                    conn.Open();
                    resultado = cmd.ExecuteNonQuery() > 0 ? true : false;
                }
            }
            catch (Exception ex)
            {
                resultado = false;
                mensaje = ex.Message;
            }

            return resultado;
        }
    }
}
