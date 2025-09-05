using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace Ecommerce.CpEntities.Models
{
    public partial class EcommerceDB : DbContext
    {
        public EcommerceDB()
            : base("name=EcommerceDB")
        {
        }

        public virtual DbSet<Categorias> Categorias { get; set; }
        public virtual DbSet<Clientes> Clientes { get; set; }
        public virtual DbSet<DetalleFacturaVentas> DetalleFacturaVentas { get; set; }
        public virtual DbSet<Empleados> Empleados { get; set; }
        public virtual DbSet<FacturaVentas> FacturaVentas { get; set; }
        public virtual DbSet<Inventarios> Inventarios { get; set; }
        public virtual DbSet<Productos> Productos { get; set; }
        public virtual DbSet<Roles> Roles { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Categorias>()
                .Property(e => e.NombreCategoria)
                .IsUnicode(false);

            modelBuilder.Entity<Categorias>()
                .Property(e => e.DescripcionCategoria)
                .IsUnicode(false);

            modelBuilder.Entity<Clientes>()
                .Property(e => e.NombreCliente)
                .IsUnicode(false);

            modelBuilder.Entity<Clientes>()
                .Property(e => e.Apellido1Cliente)
                .IsUnicode(false);

            modelBuilder.Entity<Clientes>()
                .Property(e => e.Apellido2Cliente)
                .IsUnicode(false);

            modelBuilder.Entity<Clientes>()
                .Property(e => e.CorreoCliente)
                .IsUnicode(false);

            modelBuilder.Entity<Clientes>()
                .Property(e => e.Clave)
                .IsUnicode(false);

            modelBuilder.Entity<Empleados>()
                .Property(e => e.CodigoEmpleado)
                .IsUnicode(false);

            modelBuilder.Entity<Empleados>()
                .Property(e => e.NombreEmpleado)
                .IsUnicode(false);

            modelBuilder.Entity<Empleados>()
                .Property(e => e.Apellido1Empleado)
                .IsUnicode(false);

            modelBuilder.Entity<Empleados>()
                .Property(e => e.Apellido2Empleado)
                .IsUnicode(false);

            modelBuilder.Entity<Empleados>()
                .Property(e => e.TelefonoEmpleado)
                .IsUnicode(false);

            modelBuilder.Entity<Empleados>()
                .Property(e => e.DireccionEmpleado)
                .IsUnicode(false);

            modelBuilder.Entity<Empleados>()
                .Property(e => e.CorreoEmpleado)
                .IsUnicode(false);

            modelBuilder.Entity<Empleados>()
                .Property(e => e.Clave)
                .IsUnicode(false);

            modelBuilder.Entity<FacturaVentas>()
                .Property(e => e.CodigoFactura)
                .IsUnicode(false);

            modelBuilder.Entity<FacturaVentas>()
                .Property(e => e.TipoFactura)
                .IsUnicode(false);

            modelBuilder.Entity<FacturaVentas>()
                .Property(e => e.DescripcionVentas)
                .IsUnicode(false);

            modelBuilder.Entity<Productos>()
                .Property(e => e.CodigoProducto)
                .IsUnicode(false);

            modelBuilder.Entity<Productos>()
                .Property(e => e.NombreProducto)
                .IsUnicode(false);

            modelBuilder.Entity<Productos>()
                .Property(e => e.DescripcionProducto)
                .IsUnicode(false);

            modelBuilder.Entity<Productos>()
                .Property(e => e.Precio)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Productos>()
                .Property(e => e.NombreImagen)
                .IsUnicode(false);

            modelBuilder.Entity<Roles>()
                .Property(e => e.NombreRol)
                .IsUnicode(false);

            modelBuilder.Entity<Roles>()
                .Property(e => e.DescripcionRol)
                .IsUnicode(false);
        }
    }
}
