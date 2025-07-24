namespace Ecommerce.CpEntities.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("sr.Empleados")]
    public partial class Empleados
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Empleados()
        {
            FacturaVentas = new HashSet<FacturaVentas>();
        }

        [Key]
        public int IdEmpleado { get; set; }

        [Required]
        [StringLength(10)]
        public string CodigoEmpleado { get; set; }

        [Required]
        [StringLength(100)]
        public string NombreEmpleado { get; set; }

        [Required]
        [StringLength(100)]
        public string Apellido1Empleado { get; set; }

        [StringLength(100)]
        public string Apellido2Empleado { get; set; }

        [StringLength(25)]
        public string TelefonoEmpleado { get; set; }

        [StringLength(255)]
        public string DireccionEmpleado { get; set; }

        [Required]
        [StringLength(100)]
        public string CorreoEmpleado { get; set; }

        [StringLength(100)]
        public string Clave { get; set; }

        public bool? Estado { get; set; }

        public bool? Reestablecer { get; set; }

        public DateTime? FechaRegistro { get; set; }

        public int? IdRol { get; set; }

        public virtual Roles Roles { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FacturaVentas> FacturaVentas { get; set; }
    }
}
