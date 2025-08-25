using ClosedXML.Excel;
using Ecommerce.CpEntities.Models;
using Ecommerce.CpNegocio.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ecommerce.WebApplication.Controllers
{
    
    public class DashboardController : Controller
    {


        // GET: Dashboard
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult VistaDashboard()
        {
            Dashboard dashboard = new ReporteService().VerDashboard();

            return Json(new { resultado = dashboard }, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public JsonResult ListarReporte(string fechaInicio, string fechaFin, string codigoTransaccion)
        {
            List<Reporte> reporteList = new List<Reporte>();

            reporteList = new ReporteService().ReporteVenta(fechaInicio, fechaFin, codigoTransaccion);

            return Json(new { data = reporteList }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public FileResult ExportarVentaExcel(string fechaInicio, string fechaFin, string codigoTransaccion)
        {
            List<Reporte> reportes = new List<Reporte>();
            reportes = new ReporteService().ReporteVenta(fechaInicio, fechaFin, codigoTransaccion);

            DataTable dt = new DataTable();
            dt.Locale = new System.Globalization.CultureInfo("es-NI");
            dt.Columns.Add("Fecha Venta", typeof(string));
            dt.Columns.Add("Código Factura", typeof(string));
            dt.Columns.Add("Cliente", typeof(string));
            dt.Columns.Add("Producto", typeof(string));
            dt.Columns.Add("Cantidad", typeof(int));
            dt.Columns.Add("Precio", typeof(decimal));
            //dt.Columns.Add("Subtotal", typeof(decimal));
            //dt.Columns.Add("Metodo de Pago", typeof(string));
            dt.Columns.Add("Total", typeof(decimal));

            foreach (Reporte rp in reportes)
            {
                dt.Rows.Add(new object[]
                {
                    rp.FechaVenta,
                    rp.CodigoFactura,
                    rp.Cliente,
                    rp.Producto,
                    rp.Cantidad,
                    rp.Precio,
                    //rp.Subtotal,
                    //rp.MetodoPago,
                    rp.Total
                });
            }

            dt.TableName = "Datos Ventas";
            using (var wbk = new XLWorkbook())
            {
                wbk.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wbk.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ReporteVenta" + DateTime.Now.ToString() + ".xlsx");
                }
            }
        }
    }
}