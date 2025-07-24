namespace Ecommerce.CpEntities.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("sr.DetalleFacturaVentas")]
    public partial class DetalleFacturaVentas
    {
        [Key]
        public int IdDetalleVenta { get; set; }

        public int? IdFacturaVenta { get; set; }

        public int? IdProducto { get; set; }

        public int? Cantidad { get; set; }

        public double? PrecioUnitario { get; set; }

        public double? DescuentoVentas { get; set; }

        public double? MontoTotalVentas { get; set; }

        public virtual FacturaVentas FacturaVentas { get; set; }

        public virtual Productos Productos { get; set; }
    }
}
