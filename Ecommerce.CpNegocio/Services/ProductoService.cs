using Ecommerce.CpDatos.Interfaces;
using Ecommerce.CpEntities.Models;
using Ecommerce.CpNegocio.Interfaces;
using System;
using System.Collections.Generic;

namespace Ecommerce.CpNegocio.Services
{
    public class ProductoService : IProductoService
    {
        private readonly IProductoRepositorio _productoRepo;

        public ProductoService(IProductoRepositorio productoRepositorio)
        {
            _productoRepo = productoRepositorio ?? throw new ArgumentNullException(nameof(productoRepositorio));
        }

        public int Crear(Productos producto, out string mensaje)
        {
            mensaje = ValidarProducto(producto, esActualizacion: false);
            if (!string.IsNullOrEmpty(mensaje))
                return 0;

            return _productoRepo.Insertar(producto, out mensaje);
        }

        public bool Actualizar(Productos producto, out string mensaje)
        {
            if (producto.IdProducto <= 0)
            {
                throw new ArgumentException("El producto debe tener un Id válido.");
            }

            mensaje = ValidarProducto(producto, esActualizacion: true);
            if (!string.IsNullOrEmpty(mensaje))
                return false;

            return _productoRepo.Actualizar(producto, out mensaje);
        }

        public bool Eliminar(int id, out string mensaje)
        {
            if (id <= 0)
                throw new ArgumentException("Id inválido para eliminar.");

            return _productoRepo.Eliminar(id, out mensaje);
        }

        public bool GuardarImagen(Productos producto, out string mensaje)
        {
            return _productoRepo.GuardarImagen(producto, out mensaje);
        }

        public List<Productos> ListarProducto()
        {
            return _productoRepo.Listar();
        }

        public Productos ObtenerPorId(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Id inválido para buscar producto.");

            return _productoRepo.Listar().Find(p => p.IdProducto == id);
        }

        /// <summary>
        /// Valida los campos de un producto.
        /// </summary>
        /// <param name="producto">Producto a validar.</param>
        /// <param name="esActualizacion">Indica si la validación es para actualizar.</param>
        /// <returns>Mensaje de error, vacío si todo es válido.</returns>
        private string ValidarProducto(Productos producto, bool esActualizacion)
        {
            if (producto == null)
                return "El producto no puede ser nulo.";

            if (string.IsNullOrWhiteSpace(producto.CodigoProducto))
                return "El campo Código del Producto es obligatorio.";

            if (string.IsNullOrWhiteSpace(producto.NombreProducto))
                return "El campo Nombre del Producto es obligatorio.";

            if (string.IsNullOrWhiteSpace(producto.DescripcionProducto))
                return "El campo Descripción del Producto es obligatorio.";

            int categoriaId = esActualizacion && producto.Categorias != null
                                ? producto.Categorias.IdCategoria
                                : producto.IdCategoria ?? 0;

            if (categoriaId <= 0)
                return "Debe seleccionar una categoría válida.";

            if (producto.Precio <= 0)
                return "El precio debe ser mayor que cero.";

            return string.Empty; // Todo válido
        }
    }
}
