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
    public class EmpleadoRepositorio : IEmpleadoRepositorio
    {
        private EcommerceDB objCapaEntidad;

        public EmpleadoRepositorio()
        {
        }

        public EmpleadoRepositorio(EcommerceDB objCapaEntidad)
        {
            this.objCapaEntidad = objCapaEntidad;
        }

        public bool Actualizar(Empleados empleados, out string mensaje)
        {
            bool resultado;
            mensaje = string.Empty;

            try
            {
                using (var conn = DbConnectionHelper.GetConnection())
                using (var cmd = new SqlCommand("sp_actualizarEmpleado", conn))
                {
                    cmd.Parameters.AddWithValue("CodigoEmpleado", empleados.CodigoEmpleado);
                    cmd.Parameters.AddWithValue("NombreEmpleado", empleados.NombreEmpleado);
                    cmd.Parameters.AddWithValue("Apellido1Empleado", empleados.Apellido1Empleado);
                    cmd.Parameters.AddWithValue("Apellido2Empleado", empleados.Apellido2Empleado);
                    cmd.Parameters.AddWithValue("DireccionEmpleado", empleados.DireccionEmpleado);
                    cmd.Parameters.AddWithValue("TelefonoEmpleado", empleados.TelefonoEmpleado);
                    cmd.Parameters.AddWithValue("CorreoEmpleado", empleados.CorreoEmpleado);
                    cmd.Parameters.AddWithValue("IdRol", empleados.Roles.IdRol);
                    cmd.Parameters.AddWithValue("Estado", empleados.Estado);
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

        public bool CambiarContraseña(int idUsuario, string nuevaContra, out string mensaje)
        {
            bool resultado;
            mensaje = string.Empty;

            try
            {
                using (var conn = DbConnectionHelper.GetConnection())
                using (var cmd = new SqlCommand("UPDATE sr.Empleados SET Clave = @nuevaContra, Reestablecer = 0 where IdEmpleado = @Id", conn))
                {
                    cmd.Parameters.AddWithValue("@Id", idUsuario);
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
                using (var cmd = new SqlCommand("sp_eliminarEmpleado", conn))
                {
                    cmd.Parameters.AddWithValue("IdEmpleado", id);
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

        public int Insertar(Empleados empleados, out string mensaje)
        {
            int idAuto;
            mensaje = string.Empty;
            try
            {
                using (var conn = DbConnectionHelper.GetConnection())
                using (var cmd = new SqlCommand("sp_registrarEmpleado", conn))
                {
                    cmd.Parameters.AddWithValue("CodigoEmpleado", empleados.CodigoEmpleado);
                    cmd.Parameters.AddWithValue("NombreEmpleado", empleados.NombreEmpleado);
                    cmd.Parameters.AddWithValue("Apellido1Empleado", empleados.Apellido1Empleado);
                    cmd.Parameters.AddWithValue("Apellido2Empleado", empleados.Apellido2Empleado);
                    cmd.Parameters.AddWithValue("IdRol", empleados.Roles.IdRol);
                    cmd.Parameters.AddWithValue("DireccionEmpleado", empleados.DireccionEmpleado);
                    cmd.Parameters.AddWithValue("TelefonoEmpleado", empleados.TelefonoEmpleado);
                    cmd.Parameters.AddWithValue("CorreoEmpleado", empleados.CorreoEmpleado);
                    cmd.Parameters.AddWithValue("ClaveEmpleado", empleados.Clave);
                    cmd.Parameters.AddWithValue("Estado", empleados.Estado);
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

        public Empleados ObtenerPorId(int id)
        {
            throw new NotImplementedException();
        }

        public List<Empleados> ObtenerTodos()
        {
            var listEmpleado = new List<Empleados>();

            try
            {
                using (SqlConnection conn = DbConnectionHelper.GetConnection())
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("SELECT e.IdEmpleado, e.CodigoEmpleado, e.NombreEmpleado,");
                    sb.AppendLine("e.Apellido1Empleado, e.Apellido2Empleado,");
                    sb.AppendLine("e.DireccionEmpleado, e.TelefonoEmpleado,");
                    sb.AppendLine("e.CorreoEmpleado, e.Clave, e.Estado, e.Reestablecer,");
                    sb.AppendLine("r.IdRol, r.NombreRol");
                    sb.AppendLine("FROM sr.Empleados e");
                    sb.AppendLine("INNER JOIN sr.Roles r ON r.IdRol = e.IdRol");

                    SqlCommand cmd = new SqlCommand(sb.ToString(), conn)
                    {
                        CommandType = CommandType.Text
                    };
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var empleado = new Empleados
                            {
                                IdEmpleado = Convert.ToInt32(reader["IdEmpleado"]),
                                CodigoEmpleado = reader["CodigoEmpleado"].ToString(),
                                NombreEmpleado = reader["NombreEmpleado"].ToString(),
                                Apellido1Empleado = reader["Apellido1Empleado"].ToString(),
                                Apellido2Empleado = reader["Apellido2Empleado"].ToString(),
                                DireccionEmpleado = reader["DireccionEmpleado"].ToString(),
                                TelefonoEmpleado = reader["TelefonoEmpleado"].ToString(),
                                CorreoEmpleado = reader["CorreoEmpleado"].ToString(),
                                Clave = reader["Clave"].ToString(),   // 🔑 aquí ahora sí
                                Estado = Convert.ToBoolean(reader["Estado"]),
                                Reestablecer = reader["Reestablecer"] == DBNull.Value
                                               ? (bool?)null
                                               : Convert.ToBoolean(reader["Reestablecer"]),
                                Roles = new Roles
                                {
                                    IdRol = Convert.ToInt32(reader["IdRol"]),
                                    NombreRol = reader["NombreRol"].ToString(),
                                }
                            };
                            listEmpleado.Add(empleado);
                        }
                    }
                }
            }
            catch
            {
                listEmpleado = new List<Empleados>();
            }

            return listEmpleado;
        }


        public bool ReestablecerContraseña(int idUsuario, string nuevaContra, out string mensaje)
        {
            bool resultado;
            mensaje = string.Empty;

            try
            {
                using (var conn = DbConnectionHelper.GetConnection())
                using (var cmd = new SqlCommand("UPDATE sr.Empleados SET Clave = @clave, Reestablecer = 1 where IdEmpleado = @Id", conn))
                {
                    cmd.Parameters.AddWithValue("@Id", idUsuario);
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
