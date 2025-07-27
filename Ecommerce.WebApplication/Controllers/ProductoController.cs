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

        public ProductoController()
        {
            var repositorio = new ProductoRepositorio();
            _productoService = new ProductoService(repositorio);
        }

        // GET: Producto
        public ActionResult Index()
        {
            return View();
        }

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

        [HttpPost]
        public JsonResult GuardarProducto(string objeto, HttpPostedFileBase fileImagen)
        {
            string mensaje = string.Empty;
            bool operacionExitosa = true;
            bool guardarImagen = true;

            if (string.IsNullOrEmpty(objeto))
                return Json(new { success = false, message = "Datos del producto vacíos." }, JsonRequestBehavior.AllowGet);

            var producto = JsonConvert.DeserializeObject<Productos>(objeto);
            if (producto == null)
                return Json(new { success = false, message = "No se pudo procesar el objeto del platillo." }, JsonRequestBehavior.AllowGet);

            if (!decimal.TryParse(producto.PrecioTexto, NumberStyles.AllowDecimalPoint, new CultureInfo("es-NI"), out decimal precio))
                return Json(new { success = false, message = "El formato del precio debe ser ##.##" }, JsonRequestBehavior.AllowGet);

            producto.Precio = precio;

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
                if (fileImagen != null)
                {
                    string rutaImagen = ConfigurationManager.AppSettings["ServidorFotos"];
                    string extension = Path.GetExtension(fileImagen.FileName);
                    string nombreImagen = string.Concat(producto.NombreProducto.ToString(), extension);

                    try
                    {
                        fileImagen.SaveAs(Path.Combine(rutaImagen, nombreImagen));
                    }
                    catch (Exception exImg)
                    {
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
                        mensaje += "Se guardo el producto, pero hubo un error con la imagen";
                }
            }

            return Json(new { operacionExitosa = operacionExitosa, idGenerado = producto.IdProducto, message = mensaje }, JsonRequestBehavior.AllowGet);
        }

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

            return new JsonResult
            {
                Data = new { conversion = conversion, textobase64 = textoBase64, extension = extension },
                MaxJsonLength = int.MaxValue,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };

        }
    }
}