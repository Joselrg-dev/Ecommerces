namespace Ecommerce.CpEntities.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("sr.Categorias")]
    public partial class Categorias
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Categorias()
        {
            Productos = new HashSet<Productos>();
        }

        [Key]
        public int IdCategoria { get; set; }

        [Required]
        [StringLength(100)]
        public string NombreCategoria { get; set; }

        [Column(TypeName = "text")]
        public string DescripcionCategoria { get; set; }

        public bool? Estado { get; set; }

        public DateTime? FechaRegistro { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Productos> Productos { get; set; }
    }
}
