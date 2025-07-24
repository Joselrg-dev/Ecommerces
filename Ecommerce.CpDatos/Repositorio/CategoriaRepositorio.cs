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
    public class CategoriaRepositorio : ICategoriaRepositorio
    {
        public bool Actualizar(Categorias categorias, out string mensaje)
        {
            bool resultado;
            mensaje = string.Empty;

            try
            {
                using (var conn = DbConnectionHelper.GetConnection())
                using (var cmd = new SqlCommand("sp_actualizarCategoria"))
                {
                    cmd.Parameters.AddWithValue("IdCategoria", categorias.IdCategoria);
                    cmd.Parameters.AddWithValue("NombreCategoria", categorias.NombreCategoria);
                    cmd.Parameters.AddWithValue("DescripcionCategoria", categorias.DescripcionCategoria);
                    cmd.Parameters.AddWithValue("Estado", categorias.Estado);
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
                using (var cmd = new SqlCommand("sp_eliminarCategoria", conn))
                {
                    cmd.Parameters.AddWithValue("IdCategoria", id);
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

        public int Insertar(Categorias categorias, out string mensaje)
        {
            int idGenerado;
            mensaje = string.Empty;

            try
            {
                using (var conn = DbConnectionHelper.GetConnection())
                using (var cmd = new SqlCommand("sp_registrarCategoria", conn))
                {
                    cmd.Parameters.AddWithValue("NombreCategoria", categorias.NombreCategoria);
                    cmd.Parameters.AddWithValue("DescripcionCategoria", categorias.DescripcionCategoria);
                    cmd.Parameters.AddWithValue("Estado", categorias.Estado);
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

        public List<Categorias> Listar()
        {
            var listaCatg = new List<Categorias>();
            using (var conn = DbConnectionHelper.GetConnection())
            using (var cmd = new SqlCommand("SELECT IdCategoria, NombreCategoria, DescripcionCategoria, Estado FROM sr.Categorias", conn))
            {
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var categ = new Categorias
                        {
                            IdCategoria = Convert.ToInt32(reader["IdCategoria"]),
                            NombreCategoria = reader["NombreCategoria"].ToString(),
                            DescripcionCategoria = reader["DescripcionCategoria"].ToString(),
                            Estado = Convert.ToBoolean(reader["Estado"])
                        };
                        listaCatg.Add(categ);
                    }
                }
            }

            return listaCatg;
        }

        public Categorias ObtenerPorId(int id)
        {
            throw new NotImplementedException();
        }
    }
}
