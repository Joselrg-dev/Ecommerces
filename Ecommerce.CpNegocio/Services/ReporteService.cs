using Ecommerce.CpDatos.Repositorio;
using Ecommerce.CpEntities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.CpNegocio.Services
{
    public class ReporteService
    {
        private readonly ReporteRepositorio objCapaDato = new ReporteRepositorio();

        public Dashboard VerDashboard()
        {
            return objCapaDato.VerDashboard();
        }
        public List<Reporte> ReporteVenta(string fechaInicio, string fechaFin, string codigoTransaccion)
        {
            return objCapaDato.ObtenerReporteVenta(fechaInicio, fechaFin, codigoTransaccion);
        }
    }
}
