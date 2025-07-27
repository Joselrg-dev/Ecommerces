using Ecommerce.CpDatos.Interfaces;
using Ecommerce.CpEntities.Models;
using Ecommerce.CpNegocio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.CpNegocio.Services
{
    public class ProductoService : IProductoService
    {
        private readonly IProductoRepositorio _productoRepo;

        public ProductoService(IProductoRepositorio productoRepositorio)
        {
            _productoRepo = productoRepositorio;
        }

        public bool Actualizar(Productos productos, out string mensaje)
        {
            mensaje = string.Empty;

            if (productos.IdProducto <= 0)
                throw new ArgumentException("El producto debe tener un Id válido.");

            if (string.IsNullOrEmpty(productos.CodigoProducto) || string.IsNullOrWhiteSpace(productos.CodigoProducto))
                mensaje = ("El campo código del Producto es requerido.");
            else if (string.IsNullOrEmpty(productos.NombreProducto) || string.IsNullOrWhiteSpace(productos.NombreProducto))
                mensaje = "El campo nombre es requerido.";
            else if (string.IsNullOrEmpty(productos.DescripcionProducto) || string.IsNullOrWhiteSpace(productos.DescripcionProducto))
                mensaje = "El campo primer apellido es requerido.";
            else if (productos.Categorias.IdCategoria == 0)
                mensaje = ("Debe de seleccionar una categoria.");
            else if (productos.Precio <= 0)
                mensaje = ("El precio debe ser mayor que cero.");

            if (string.IsNullOrEmpty(mensaje))
                return _productoRepo.Actualizar(productos, out mensaje);
            else
                return false;
        }

        public int Crear(Productos productos, out string mensaje)
        {
            mensaje = string.Empty;

            if (string.IsNullOrEmpty(productos.CodigoProducto) || string.IsNullOrWhiteSpace(productos.CodigoProducto))
                mensaje = ("El campo código del Producto es requerido.");
            else if (string.IsNullOrEmpty(productos.NombreProducto) || string.IsNullOrWhiteSpace(productos.NombreProducto))
                mensaje = "El campo nombre es requerido.";
            else if (string.IsNullOrEmpty(productos.DescripcionProducto) || string.IsNullOrWhiteSpace(productos.DescripcionProducto))
                mensaje = "El campo primer apellido es requerido.";
            else if (productos.IdCategoria == 0)
                mensaje = ("Debe de seleccionar una categoria.");
            else if (productos.Precio <= 0)
                mensaje = ("El precio debe ser mayor que cero.");

            if (string.IsNullOrEmpty(mensaje))
                return _productoRepo.Insertar(productos, out mensaje);
            else
                return 0;
        }

        public bool Eliminar(int id, out string mensaje)
        {
            if (id <= 0)
                throw new ArgumentException("Id inválido para eliminar.");
            else
                return _productoRepo.Eliminar(id, out mensaje);
        }

        public bool GuardarImagen(Productos productos, out string mensaje)
        {
            return _productoRepo.GuardarImagen(productos, out mensaje);
        }

        public List<Productos> ListarProducto()
        {
            return _productoRepo.Listar();
        }

        public Productos ObtenerPorId(int id)
        {
            throw new NotImplementedException();
        }
    }
}
