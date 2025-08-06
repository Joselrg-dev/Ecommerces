using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.CpEntities.Models
{
    public class Reporte
    {
        public string FechaVenta { get; set; }
        public string CodigoFactura { get; set; }
        public string Cliente { get; set; }
        public string Producto { get; set; }
        public int Cantidad { get; set; }
        public decimal Precio { get; set; }
        public decimal Subtotal { get; set; }
        public string MetodoPago { get; set; }
        public decimal Total { get; set; }
    }

}