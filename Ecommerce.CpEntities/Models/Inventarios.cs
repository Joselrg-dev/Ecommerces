namespace Ecommerce.CpEntities.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("sr.Inventarios")]
    public partial class Inventarios
    {
        [Key]
        public int IdInventario { get; set; }

        public int? IdProducto { get; set; }

        public int? StockProductos { get; set; }

        public virtual Productos Productos { get; set; }
    }
}
