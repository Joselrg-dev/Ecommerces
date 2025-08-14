using Ecommerce.CpCommons;
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
    public class ReporteRepositorio
    {
        public Dashboard VerDashboard()
        {
            Dashboard dashboard = new Dashboard();

            try
            {
                using (var conn = DbConnectionHelper.GetConnection())
                using (var cmd = new SqlCommand("sp_reporteDashboard", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            dashboard = new Dashboard()
                            {
                                TotalCliente = Convert.ToInt32(reader["TotalCliente"]),
                                TotalVentas = Convert.ToInt32(reader["TotalVentas"]),
                                TotalProductos = Convert.ToInt32(reader["TotalProductos"]),
                            };
                        }
                    }
                }
            }
            catch
            {
                dashboard = new Dashboard();
            }

            return dashboard;
        }

        public List<Reporte> ObtenerReporteVenta(string fechaInicio, string fechaFin, string codigoTransaccion)
        {
            var listReport = new List<Reporte>();
            using (var conn = DbConnectionHelper.GetConnection())
            using (var cmd = new SqlCommand("sp_reporteVentas", conn))
            {
                cmd.Parameters.Add("@FechaInicio", SqlDbType.VarChar, 10).Value = fechaInicio;
                 cmd.Parameters.Add("@FechaFin", SqlDbType.VarChar, 10).Value = fechaFin;
                cmd.Parameters.Add("@codigoTransaccion", SqlDbType.VarChar, 50)
              .Value = string.IsNullOrEmpty(codigoTransaccion) ? (object)DBNull.Value : codigoTransaccion;

                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // rept variable que almacena registro de ventas
                        var report = new Reporte
                        {
                            FechaVenta = reader["FechaVenta"].ToString(),
                            CodigoFactura = reader["CodigoFactura"].ToString(),
                            Cliente = reader["Cliente"].ToString(),
                            Producto = reader["Producto"].ToString(),
                            Cantidad = Convert.ToInt32(reader["Cantidad"]),
                            Precio = Convert.ToDecimal(reader["Precio"], new CultureInfo("es_NI")),
                            //Subtotal = Convert.ToDecimal(reader["Subtotal"], new CultureInfo("es_NI")),
                            //MetodoPago = reader["MetodoPago"].ToString(),
                            Total = Convert.ToDecimal(reader["Total"], new CultureInfo("es_NI"))
                        };

                        listReport.Add(report);
                    }
                }
            }

            return listReport;
        }
    }
}