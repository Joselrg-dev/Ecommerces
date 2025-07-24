namespace Ecommerce.CpEntities.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("sr.Roles")]
    public partial class Roles
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Roles()
        {
            Empleados = new HashSet<Empleados>();
        }

        [Key]
        public int IdRol { get; set; }

        [Required]
        [StringLength(50)]
        public string NombreRol { get; set; }

        [Column(TypeName = "text")]
        public string DescripcionRol { get; set; }

        public bool? Estado { get; set; }

        public DateTime? FechaRegistro { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Empleados> Empleados { get; set; }
    }
}
