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
    public class RolRepositorio : IRolRepositorio
    {
        public bool Actualizar(Roles roles, out string mensaje)
        {
            bool resultado;
            mensaje = string.Empty;

            try
            {
                using (var conn = DbConnectionHelper.GetConnection())
                using (var cmd = new SqlCommand("sp_actualizarRol", conn))
                {
                    cmd.Parameters.AddWithValue("IdRol", roles.IdRol);
                    cmd.Parameters.AddWithValue("NombreRol", roles.NombreRol);
                    cmd.Parameters.AddWithValue("DescripcionRol", roles.DescripcionRol);
                    cmd.Parameters.AddWithValue("Estado", roles.Estado);
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

        public bool Eliminar(int id, out string mensaje)
        {
            bool resultado;
            mensaje = string.Empty;

            try
            {
                using (var conn = DbConnectionHelper.GetConnection())
                using (var cmd = new SqlCommand("sp_eliminarRol", conn))
                {
                    cmd.Parameters.AddWithValue("IdRol", id);
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

        public int Insertar(Roles roles, out string mensaje)
        {
            int idGenerado;
            mensaje = string.Empty;

            try
            {
                using (var conn = DbConnectionHelper.GetConnection())
                using (var cmd = new SqlCommand("sp_registrarRol", conn))
                {
                    cmd.Parameters.AddWithValue("NombreRol", roles.NombreRol);
                    cmd.Parameters.AddWithValue("DescripcionRol", roles.DescripcionRol);
                    cmd.Parameters.AddWithValue("Estado", roles.Estado);
                    cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    conn.Open();
                    cmd.ExecuteNonQuery();

                    idGenerado = Convert.ToInt32(cmd.Parameters["Resultado"].Value);
                    mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                idGenerado = 0;
                mensaje = ex.Message;
            }

            return idGenerado;
        }

        public List<Roles> Listar()
        {
            var lista = new List<Roles>();
            using (var conn = DbConnectionHelper.GetConnection())
            using (var cmd = new SqlCommand("SELECT IdRol, NombreRol, DescripcionRol, Estado FROM sr.Roles", conn))
            {
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var rol = new Roles
                        {
                            IdRol = Convert.ToInt32(reader["IdRol"]),
                            NombreRol = reader["NombreRol"].ToString(),
                            DescripcionRol = reader["DescripcionRol"].ToString(),
                            Estado = Convert.ToBoolean(reader["Estado"])
                        };
                        lista.Add(rol);
                    }
                }
            }
            return lista;
        }

        public Roles ObtenerPorId(int id)
        {
            throw new NotImplementedException();
        }
    }
}
