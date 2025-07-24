namespace Ecommerce.CpEntities.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "sr.Categorias",
                c => new
                    {
                        IdCategoria = c.Int(nullable: false, identity: true),
                        NombreCategoria = c.String(nullable: false, maxLength: 100, unicode: false),
                        DescripcionCategoria = c.String(unicode: false, storeType: "text"),
                        Estado = c.Boolean(),
                        FechaRegistro = c.DateTime(),
                    })
                .PrimaryKey(t => t.IdCategoria);
            
            CreateTable(
                "sr.Productos",
                c => new
                    {
                        IdProducto = c.Int(nullable: false, identity: true),
                        CodigoProducto = c.String(nullable: false, maxLength: 10, unicode: false),
                        NombreProducto = c.String(nullable: false, maxLength: 100, unicode: false),
                        DescripcionProducto = c.String(unicode: false, storeType: "text"),
                        Cantidad = c.Int(),
                        Precio = c.Decimal(precision: 10, scale: 2),
                        ImagenUrl = c.String(),
                        NombreImagen = c.String(maxLength: 100, unicode: false),
                        Estado = c.Boolean(),
                        FechaRegistro = c.DateTime(),
                        IdCategoria = c.Int(),
                    })
                .PrimaryKey(t => t.IdProducto)
                .ForeignKey("sr.Categorias", t => t.IdCategoria)
                .Index(t => t.IdCategoria);
            
            CreateTable(
                "sr.DetalleFacturaVentas",
                c => new
                    {
                        IdDetalleVenta = c.Int(nullable: false, identity: true),
                        IdFacturaVenta = c.Int(),
                        IdProducto = c.Int(),
                        Cantidad = c.Int(),
                        PrecioUnitario = c.Double(),
                        DescuentoVentas = c.Double(),
                        MontoTotalVentas = c.Double(),
                    })
                .PrimaryKey(t => t.IdDetalleVenta)
                .ForeignKey("sr.FacturaVentas", t => t.IdFacturaVenta)
                .ForeignKey("sr.Productos", t => t.IdProducto)
                .Index(t => t.IdFacturaVenta)
                .Index(t => t.IdProducto);
            
            CreateTable(
                "sr.FacturaVentas",
                c => new
                    {
                        IdFacturaVenta = c.Int(nullable: false, identity: true),
                        CodigoFactura = c.String(nullable: false, maxLength: 10, unicode: false),
                        TipoFactura = c.String(maxLength: 1, unicode: false),
                        CantidadVentas = c.Int(),
                        PrecioVentas = c.Double(),
                        DescripcionVentas = c.String(unicode: false, storeType: "text"),
                        FechaFacturaVentas = c.DateTime(storeType: "date"),
                        IdEmpleado = c.Int(),
                        IdCliente = c.Int(),
                    })
                .PrimaryKey(t => t.IdFacturaVenta)
                .ForeignKey("sr.Clientes", t => t.IdCliente)
                .ForeignKey("sr.Empleados", t => t.IdEmpleado)
                .Index(t => t.IdEmpleado)
                .Index(t => t.IdCliente);
            
            CreateTable(
                "sr.Clientes",
                c => new
                    {
                        IdCliente = c.Int(nullable: false, identity: true),
                        CodigoCliente = c.String(nullable: false, maxLength: 100, unicode: false),
                        NombreCliente = c.String(nullable: false, maxLength: 100, unicode: false),
                        Apellido1Cliente = c.String(nullable: false, maxLength: 100, unicode: false),
                        TelefonoCliente = c.Int(),
                        CorreoCliente = c.String(maxLength: 100, unicode: false),
                        Clave = c.String(maxLength: 100, unicode: false),
                        Estado = c.Boolean(),
                        Reestablecer = c.Boolean(),
                        FechaRegistro = c.DateTime(),
                    })
                .PrimaryKey(t => t.IdCliente);
            
            CreateTable(
                "sr.Empleados",
                c => new
                    {
                        IdEmpleado = c.Int(nullable: false, identity: true),
                        CodigoEmpleado = c.String(nullable: false, maxLength: 10, unicode: false),
                        NombreEmpleado = c.String(nullable: false, maxLength: 100, unicode: false),
                        Apellido1Empleado = c.String(nullable: false, maxLength: 100, unicode: false),
                        Apellido2Empleado = c.String(maxLength: 100, unicode: false),
                        TelefonoEmpleado = c.Int(),
                        DireccionEmpleado = c.String(maxLength: 255, unicode: false),
                        CorreoEmpleado = c.String(nullable: false, maxLength: 100, unicode: false),
                        Clave = c.String(maxLength: 100, unicode: false),
                        Estado = c.Boolean(),
                        Reestablecer = c.Boolean(),
                        FechaRegistro = c.DateTime(),
                        IdRol = c.Int(),
                    })
                .PrimaryKey(t => t.IdEmpleado)
                .ForeignKey("sr.Roles", t => t.IdRol)
                .Index(t => t.IdRol);
            
            CreateTable(
                "sr.Roles",
                c => new
                    {
                        IdRol = c.Int(nullable: false, identity: true),
                        NombreRol = c.String(nullable: false, maxLength: 50, unicode: false),
                        DescripcionRol = c.String(unicode: false, storeType: "text"),
                        Estado = c.Boolean(),
                        FechaRegistro = c.DateTime(),
                    })
                .PrimaryKey(t => t.IdRol);
            
            CreateTable(
                "sr.Inventarios",
                c => new
                    {
                        IdInventario = c.Int(nullable: false, identity: true),
                        IdProducto = c.Int(),
                        StockProductos = c.Int(),
                    })
                .PrimaryKey(t => t.IdInventario)
                .ForeignKey("sr.Productos", t => t.IdProducto)
                .Index(t => t.IdProducto);
            
        }
        
        public override void Down()
        {
            DropForeignKey("sr.Inventarios", "IdProducto", "sr.Productos");
            DropForeignKey("sr.DetalleFacturaVentas", "IdProducto", "sr.Productos");
            DropForeignKey("sr.Empleados", "IdRol", "sr.Roles");
            DropForeignKey("sr.FacturaVentas", "IdEmpleado", "sr.Empleados");
            DropForeignKey("sr.DetalleFacturaVentas", "IdFacturaVenta", "sr.FacturaVentas");
            DropForeignKey("sr.FacturaVentas", "IdCliente", "sr.Clientes");
            DropForeignKey("sr.Productos", "IdCategoria", "sr.Categorias");
            DropIndex("sr.Inventarios", new[] { "IdProducto" });
            DropIndex("sr.Empleados", new[] { "IdRol" });
            DropIndex("sr.FacturaVentas", new[] { "IdCliente" });
            DropIndex("sr.FacturaVentas", new[] { "IdEmpleado" });
            DropIndex("sr.DetalleFacturaVentas", new[] { "IdProducto" });
            DropIndex("sr.DetalleFacturaVentas", new[] { "IdFacturaVenta" });
            DropIndex("sr.Productos", new[] { "IdCategoria" });
            DropTable("sr.Inventarios");
            DropTable("sr.Roles");
            DropTable("sr.Empleados");
            DropTable("sr.Clientes");
            DropTable("sr.FacturaVentas");
            DropTable("sr.DetalleFacturaVentas");
            DropTable("sr.Productos");
            DropTable("sr.Categorias");
        }
    }
}
