using Ecommerce.CpDatos.Repositorio;
using Ecommerce.CpEntities.Models;
using Ecommerce.CpNegocio.Interfaces;
using Ecommerce.CpNegocio.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ecommerce.WebApplication.Controllers
{
    
    public class CategoriaController : Controller
    {
        private readonly ICategoriaService _categoriaService;

        public CategoriaController()
        {
            var repo = new CategoriaRepositorio();
            _categoriaService = new CategoriaService(repo);
        }

        // GET: Categoria
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult ListarCategoria()
        {
            try
            {
                List<Categorias> listCatg = _categoriaService.ListarCategorias();
                return Json(new { success = true, data = listCatg }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult GuardarCategoria(Categorias categorias)
        {
            object respuesta;
            string mensaje = string.Empty;

            try
            {
                if (categorias.IdCategoria == 0)
                    respuesta = _categoriaService.Crear(categorias, out mensaje);
                else
                    respuesta = _categoriaService.Actualizar(categorias, out mensaje);

                return Json(new { respuesta, message = mensaje }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { respuesta = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult EliminarCategoria(int id)
        {
            try
            {
                string mensaje;
                bool resultado = _categoriaService.Eliminar(id, out mensaje);
                return Json(new { success = resultado, message = mensaje }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error en el servidor, contactar al administrador.", ex.Message });
            }
        }
    }
}