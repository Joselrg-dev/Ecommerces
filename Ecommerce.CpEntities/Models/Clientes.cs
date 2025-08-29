namespace Ecommerce.CpEntities.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("sr.Clientes")]
    public partial class Clientes
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Clientes()
        {
            FacturaVentas = new HashSet<FacturaVentas>();
        }

        [Key]
        public int IdCliente { get; set; }

        [Required]
        [StringLength(10)]
        public string CodigoCliente { get; set; }

        [Required]
        [StringLength(100)]
        public string NombreCliente { get; set; }

        [Required]
        [StringLength(100)]
        public string Apellido1Cliente { get; set; }

 
        [StringLength(100)]
        public string Apellido2Cliente { get; set; }

        [StringLength(25)]
        public string TelefonoCliente { get; set; }

        [StringLength(100)]
        public string CorreoCliente { get; set; }

        [StringLength(100)]
        public string Clave { get; set; }

        public bool? Estado { get; set; }

        public bool? Reestablecer { get; set; }

        public DateTime? FechaRegistro { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FacturaVentas> FacturaVentas { get; set; }
    }
}
