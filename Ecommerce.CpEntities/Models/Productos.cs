namespace Ecommerce.CpEntities.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("sr.Productos")]
    public partial class Productos
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Productos()
        {
            DetalleFacturaVentas = new HashSet<DetalleFacturaVentas>();
            Inventarios = new HashSet<Inventarios>();
        }

        [Key]
        public int IdProducto { get; set; }

        [Required]
        [StringLength(10)]
        public string CodigoProducto { get; set; }

        [Required]
        [StringLength(100)]
        public string NombreProducto { get; set; }

        [Column(TypeName = "text")]
        public string DescripcionProducto { get; set; }

        public int? Cantidad { get; set; }

        public decimal? Precio { get; set; }

        public string ImagenUrl { get; set; }

        [StringLength(100)]
        public string NombreImagen { get; set; }

        public bool? Estado { get; set; }

        public DateTime? FechaRegistro { get; set; }

        public int? IdCategoria { get; set; }

        public virtual Categorias Categorias { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DetalleFacturaVentas> DetalleFacturaVentas { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Inventarios> Inventarios { get; set; }
    }
}
