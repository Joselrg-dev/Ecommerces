using Ecommerce.CpCommons;
using Ecommerce.CpDatos.Repositorio;
using Ecommerce.CpEntities.Models;
using Ecommerce.CpNegocio.Interfaces;
using Ecommerce.CpNegocio.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ecommerce.WebApplication.Controllers
{
    public class ProductoController : Controller
    {
        private readonly IProductoService _productoService;

        // Constructor por defecto: inicializa el servicio de producto con su repositorio correspondiente
        public ProductoController()
        {
            var repositorio = new ProductoRepositorio();
            _productoService = new ProductoService(repositorio);
        }

        // GET: Producto
        // Retorna la vista principal del módulo de productos
        public ActionResult Index()
        {
            return View();
        }

        // Método para listar todos los productos en formato JSON
        [HttpGet]
        public JsonResult ListarProducto()
        {
            try
            {
                var listaProducto = _productoService.ListarProducto();
                return Json(new { success = true, data = listaProducto }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                // Captura errores inesperados y retorna mensaje al cliente
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // Método para crear o actualizar un producto
        [HttpPost]
        public JsonResult GuardarProducto(string objeto, HttpPostedFileBase fileImagen)
        {
            string mensaje = string.Empty;
            bool operacionExitosa = true;
            bool guardarImagen = true;

            // Validación de objeto recibido
            if (string.IsNullOrEmpty(objeto))
                return Json(new { success = false, message = "Datos del producto vacíos." }, JsonRequestBehavior.AllowGet);

            var producto = JsonConvert.DeserializeObject<Productos>(objeto);
            if (producto == null)
                return Json(new { success = false, message = "No se pudo procesar el objeto del platillo." }, JsonRequestBehavior.AllowGet);

            // Validación del formato del precio
            if (!decimal.TryParse(producto.PrecioTexto, NumberStyles.AllowDecimalPoint, new CultureInfo("es-NI"), out decimal precio))
                return Json(new { success = false, message = "El formato del precio debe ser ##.##" }, JsonRequestBehavior.AllowGet);

            producto.Precio = precio;

            // Crear nuevo producto o actualizar existente
            if (producto.IdProducto == 0)
            {
                int idProductoGenerado = _productoService.Crear(producto, out mensaje);

                if (idProductoGenerado != 0)
                    producto.IdProducto = idProductoGenerado;
                else
                    operacionExitosa = false;
            }
            else
                operacionExitosa = _productoService.Actualizar(producto, out mensaje);

            if (!operacionExitosa)
            {
                return Json(new { success = false, message = mensaje }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                // Manejo de la imagen del producto
                if (fileImagen != null)
                {
                    string rutaImagen = ConfigurationManager.AppSettings["ServidorFotos"];
                    string extension = Path.GetExtension(fileImagen.FileName);
                    string nombreImagen = string.Concat(producto.NombreProducto.ToString(), extension);

                    try
                    {
                        // Guardar archivo en la ruta configurada
                        fileImagen.SaveAs(Path.Combine(rutaImagen, nombreImagen));
                    }
                    catch (Exception exImg)
                    {
                        // Error al guardar imagen
                        mensaje += $" Se guardó el producto, pero hubo un error con la imagen: {exImg.Message}";
                        guardarImagen = false;
                    }

                    if (guardarImagen)
                    {
                        producto.ImagenUrl = rutaImagen;
                        producto.NombreImagen = nombreImagen;
                        bool rspta = _productoService.GuardarImagen(producto, out mensaje);
                    }
                    else
                        mensaje += " Se guardó el producto, pero hubo un error con la imagen";
                }
            }

            return Json(new { operacionExitosa = operacionExitosa, idGenerado = producto.IdProducto, message = mensaje }, JsonRequestBehavior.AllowGet);
        }

        // Método para eliminar un producto por su Id
        [HttpPost]
        public JsonResult EliminarProducto(int id)
        {
            string mensaje = string.Empty;

            try
            {
                bool resultado = _productoService.Eliminar(id, out mensaje);
                return Json(new { success = resultado, message = mensaje }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error en el servidor: {ex.Message}" }, JsonRequestBehavior.AllowGet);
            }
        }

        // Método para obtener la imagen de un producto en Base64
        [HttpPost]
        public JsonResult ImagenProducto(int id)
        {
            bool conversion = false;
            string textoBase64 = string.Empty;
            string extension = string.Empty;

            var producto = _productoService.ListarProducto().FirstOrDefault(p => p.IdProducto == id);

            if (producto != null)
            {
                string rutaImagen = Path.Combine(producto.ImagenUrl ?? "", producto.NombreImagen ?? "");
                textoBase64 = SeguridadHelpers.ConversionBase64(rutaImagen, out conversion);
                extension = Path.GetExtension(producto.NombreImagen ?? "");
            }

            // Se devuelve la información de la imagen y su estado de conversión
            return new JsonResult
            {
                Data = new { conversion = conversion, textobase64 = textoBase64, extension = extension },
                MaxJsonLength = int.MaxValue,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
    }
}
