namespace Ecommerce.CpEntities.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("sr.FacturaVentas")]
    public partial class FacturaVentas
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public FacturaVentas()
        {
            DetalleFacturaVentas = new HashSet<DetalleFacturaVentas>();
        }

        [Key]
        public int IdFacturaVenta { get; set; }

        [Required]
        [StringLength(10)]
        public string CodigoFactura { get; set; }

        [StringLength(1)]
        public string TipoFactura { get; set; }

        public int? CantidadVentas { get; set; }

        public double? PrecioVentas { get; set; }

        [Column(TypeName = "text")]
        public string DescripcionVentas { get; set; }

        [Column(TypeName = "date")]
        public DateTime? FechaFacturaVentas { get; set; }

        public int? IdEmpleado { get; set; }

        public int? IdCliente { get; set; }

        public virtual Clientes Clientes { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DetalleFacturaVentas> DetalleFacturaVentas { get; set; }

        public virtual Empleados Empleados { get; set; }
    }
}
