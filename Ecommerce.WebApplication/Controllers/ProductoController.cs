using Ecommerce.CpCommons;
using Ecommerce.CpDatos.Repositorio;
using Ecommerce.CpEntities.Models;
using Ecommerce.CpNegocio.Interfaces;
using Ecommerce.CpNegocio.Services;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ecommerce.WebApplication.Controllers
{
    /// <summary>
    /// Controlador encargado de gestionar la administración de productos del e-commerce.
    /// Incluye operaciones CRUD, validaciones de datos y gestión de imágenes asociadas.
    /// </summary>
    public class ProductoController : Controller
    {
        private readonly IProductoService _productoService;
        private readonly string _rutaImagenes;

        /// <summary>
        /// Inicializa el controlador, configurando el servicio de productos y la ruta de almacenamiento de imágenes.
        /// </summary>
        public ProductoController()
        {
            var repositorio = new ProductoRepositorio();
            _productoService = new ProductoService(repositorio);

            // NOTE: La ruta se obtiene desde Web.config (key: "ServidorFotos").
            _rutaImagenes = ConfigurationManager.AppSettings["ServidorFotos"];
        }

        #region Vistas

        /// <summary>
        /// Muestra la vista principal de gestión de productos.
        /// </summary>
        public ActionResult Index()
        {
            return View();
        }

        #endregion

        #region API - CRUD Productos

        /// <summary>
        /// Obtiene la lista de productos en formato JSON.
        /// </summary>
        /// <returns>
        /// JSON con:
        /// - success: indica éxito o fallo
        /// - data: lista de productos
        /// - message: error si aplica
        /// </returns>
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
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Crea o actualiza un producto, validando precio y guardando imagen si se adjunta.
        /// </summary>
        /// <param name="objeto">JSON serializado de un objeto Producto</param>
        /// <param name="fileImagen">Archivo de imagen (jpg, png, gif, máx. 5MB)</param>
        /// <returns>
        /// JSON con:
        /// - success: indica éxito o fallo
        /// - idGenerado: Id del producto creado/actualizado
        /// - message: mensajes adicionales o errores
        /// </returns>
        [HttpPost]
        public JsonResult GuardarProducto(string objeto, HttpPostedFileBase fileImagen)
        {
            if (string.IsNullOrEmpty(objeto))
                return Json(new { success = false, message = "Datos del producto vacíos." }, JsonRequestBehavior.AllowGet);

            Productos producto;
            try
            {
                producto = JsonConvert.DeserializeObject<Productos>(objeto);
            }
            catch
            {
                return Json(new { success = false, message = "Objeto inválido." }, JsonRequestBehavior.AllowGet);
            }

            if (producto == null)
                return Json(new { success = false, message = "No se pudo procesar el producto." }, JsonRequestBehavior.AllowGet);

            // Validación y conversión del precio (ejemplo: "25.50" con cultura nicaragüense)
            if (!decimal.TryParse(producto.PrecioTexto, NumberStyles.AllowDecimalPoint, new CultureInfo("es-NI"), out decimal precio))
                return Json(new { success = false, message = "El formato del precio debe ser ##.##" }, JsonRequestBehavior.AllowGet);

            producto.Precio = precio;

            string mensaje = string.Empty;
            bool operacionExitosa = producto.IdProducto == 0
                                    ? _productoService.Crear(producto, out mensaje) > 0
                                    : _productoService.Actualizar(producto, out mensaje);

            if (!operacionExitosa)
                return Json(new { success = false, message = mensaje }, JsonRequestBehavior.AllowGet);

            #region Gestión de imágenes
            if (fileImagen != null)
            {
                var validExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                string extension = Path.GetExtension(fileImagen.FileName)?.ToLower();

                if (!validExtensions.Contains(extension))
                {
                    mensaje += " Tipo de imagen no permitido.";
                }
                else if (fileImagen.ContentLength > 5 * 1024 * 1024) // límite de 5MB
                {
                    mensaje += " Tamaño de imagen demasiado grande.";
                }
                else
                {
                    try
                    {
                        // Se genera un nombre único para evitar colisiones
                        string nombreUnico = $"{Guid.NewGuid()}{extension}";
                        string rutaCompleta = Path.Combine(_rutaImagenes, nombreUnico);

                        fileImagen.SaveAs(rutaCompleta);

                        // Guardar datos de imagen en BD
                        producto.ImagenUrl = _rutaImagenes;
                        producto.NombreImagen = nombreUnico;
                        _productoService.GuardarImagen(producto, out string imgMensaje);

                        if (!string.IsNullOrEmpty(imgMensaje))
                            mensaje += $" {imgMensaje}";
                    }
                    catch (Exception exImg)
                    {
                        mensaje += $" Error al guardar la imagen: {exImg.Message}";
                    }
                }
            }
            #endregion

            return Json(new { success = true, idGenerado = producto.IdProducto, message = mensaje }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Elimina un producto según su Id.
        /// </summary>
        [HttpPost]
        public JsonResult EliminarProducto(int id)
        {
            try
            {
                bool resultado = _productoService.Eliminar(id, out string mensaje);
                return Json(new { success = resultado, message = mensaje }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error en el servidor: {ex.Message}" }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Devuelve la imagen en base64 para mostrarla en la UI.
        /// </summary>
        [HttpPost]
        public JsonResult ImagenProducto(int id)
        {
            var producto = _productoService.ListarProducto().FirstOrDefault(p => p.IdProducto == id);
            if (producto == null)
                return Json(new { conversion = false, textobase64 = string.Empty, extension = string.Empty }, JsonRequestBehavior.AllowGet);

            string rutaImagen = Path.Combine(producto.ImagenUrl ?? "", producto.NombreImagen ?? "");
            string base64 = SeguridadHelpers.ConversionBase64(rutaImagen, out bool conversion);
            string extension = Path.GetExtension(producto.NombreImagen ?? "");

            return Json(new { conversion, textobase64 = base64, extension }, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}
