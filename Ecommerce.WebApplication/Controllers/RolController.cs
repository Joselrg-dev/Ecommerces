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
    public class RolController : Controller
    {
        private readonly IRolServices _rolService;

        public RolController()
        {
            var repositorio = new RolRepositorio();
            _rolService = new RolServices(repositorio);
        }

        // GET: Rol
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult ListarRol()
        {
            try
            {
                List<Roles> lista = _rolService.ListarTodo();
                return Json(new { success = true, data = lista }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult GuardarRol(Roles roles)
        {
            object respuesta;
            string mensaje = string.Empty;

            try
            {
                if (roles.IdRol == 0)
                    respuesta = _rolService.Crear(roles, out mensaje);
                else
                    respuesta = _rolService.Actualizar(roles, out mensaje);
            }
            catch (Exception ex)
            {
                return Json(new { respuesta = false, mensaje = ex.Message }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { respuesta, message = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EliminarRol(int id)
        {
            try
            {
                string mensaje;
                bool resultado = _rolService.Eliminar(id, out mensaje);
                return Json(new { success = resultado, message = mensaje }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error en el servidor, contactar al administrador.", ex.Message });
            }
        }
    }
}