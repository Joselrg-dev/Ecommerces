using Ecommerce.CpCommons;
using Ecommerce.CpDatos.Interfaces;
using Ecommerce.CpEntities.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.CpDatos.Repositorio
{
    public class ProductoRepositorio : IProductoRepositorio
    {
        public bool Actualizar(Productos productos, out string mensaje)
        {
            bool resultado;
            mensaje = string.Empty;
            try
            {
                using (var conn = DbConnectionHelper.GetConnection())
                using (var cmd = new SqlCommand("sp_actualizarProducto", conn))
                {
                    cmd.Parameters.AddWithValue("IdProducto", productos.IdProducto);
                    cmd.Parameters.AddWithValue("CodigoProducto", productos.CodigoProducto);
                    cmd.Parameters.AddWithValue("NombreProducto", productos.NombreProducto);
                    cmd.Parameters.AddWithValue("DescripcionProducto", productos.DescripcionProducto);
                    cmd.Parameters.AddWithValue("Cantidad", productos.Cantidad);
                    cmd.Parameters.AddWithValue("Precio", productos.Precio);
                    cmd.Parameters.AddWithValue("IdCategoria", productos.IdCategoria);
                    cmd.Parameters.AddWithValue("Estado", productos.Estado);
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
                using (var cmd = new SqlCommand("sp_eliminarProducto", conn))
                {
                    cmd.Parameters.AddWithValue("IdProducto", id);
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

        public bool GuardarImagen(Productos productos, out string mensaje)
        {
            bool resultado = false;
            mensaje = string.Empty;
            try
            {
                string query = "UPDATE sr.Productos SET ImagenUrl = @imagenUrl, NombreImagen = @nombreImagen where IdProducto = @IdProducto";
                using (var conn = DbConnectionHelper.GetConnection())
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@imagenUrl", productos.ImagenUrl);
                    cmd.Parameters.AddWithValue("@nombreImagen", productos.NombreImagen);
                    cmd.Parameters.AddWithValue("@IdProducto", productos.IdProducto);
                    cmd.CommandType = CommandType.Text;

                    conn.Open();
                    if (cmd.ExecuteNonQuery() > 0)
                        resultado = true;
                    else
                        mensaje = "No se puede actualizar imagen";
                }
            }
            catch (Exception ex)
            {
                resultado = false;
                mensaje = ex.Message;
            }

            return resultado;
        }
    

        public int Insertar(Productos productos, out string mensaje)
        {
            int idGenerado;
            mensaje = string.Empty;
            try
            {
                using (var conn = DbConnectionHelper.GetConnection())
                using (var cmd = new SqlCommand("sp_registrarProducto", conn))
                {
                    cmd.Parameters.AddWithValue("CodigoProducto", productos.CodigoProducto);
                    cmd.Parameters.AddWithValue("NombreProducto", productos.NombreProducto);
                    cmd.Parameters.AddWithValue("DescripcionProducto", productos.DescripcionProducto);
                    cmd.Parameters.AddWithValue("Cantidad", productos.Cantidad);
                    cmd.Parameters.AddWithValue("Precio", productos.Precio);
                    cmd.Parameters.AddWithValue("IdCategoria", productos.IdCategoria);
                    cmd.Parameters.AddWithValue("Estado", productos.Estado);
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

        public List<Productos> Listar()
        {
            var lista = new List<Productos>();

            try
            {
                using (SqlConnection conn = DbConnectionHelper.GetConnection())
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("select pr.CodigoProducto as 'Código', pr.NombreProducto as 'Producto',");
                    sb.AppendLine("pr.DescripcionProducto as 'Descripción', pr.Cantidad 'Cantidad',");
                    sb.AppendLine("pr.Precio as 'Precio', pr.ImagenUrl as 'Url',");
                    sb.AppendLine("pr.NombreImagen as 'Imagen', ct.IdCategoria, ct.NombreCategoria 'Categoria', pr.Estado");
                    sb.AppendLine("from sr.Productos pr");
                    sb.AppendLine("inner join sr.Categorias ct on ct.IdCategoria = pr.IdCategoria");


                    SqlCommand cmd = new SqlCommand(sb.ToString(), conn)
                    {
                        CommandType = CommandType.Text
                    };

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var producto = new Productos
                            {
                                CodigoProducto = reader["Código"].ToString(),
                                NombreProducto = reader["Producto"].ToString(),
                                DescripcionProducto = reader["Descripción"].ToString(),
                                Cantidad = Convert.ToInt32(reader["Cantidad"]),
                                Precio = Convert.ToDecimal(reader["Precio"], new CultureInfo("es_NI")),
                                ImagenUrl = reader["Url"].ToString(),
                                NombreImagen = reader["Imagen"].ToString(),
                                Categorias = new Categorias
                                {
                                    IdCategoria = Convert.ToInt32(reader["IdCategoria"]),
                                    NombreCategoria = reader["Categoria"].ToString()
                                },
                                Estado = Convert.ToBoolean(reader["Estado"])
                            };
                            lista.Add(producto);
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                lista = new List<Productos>();
                // Log o lanza
                throw new Exception("Error al listar productos", ex);
            }

            return lista;
        }

        public Productos ObtenerPorId(int id)
        {
            throw new NotImplementedException();
        }


    }
}
