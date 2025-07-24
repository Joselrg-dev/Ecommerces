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
    public class EmpleadoController : Controller
    {
        private readonly IEmpleadoService _empleadoService;

        public EmpleadoController()
        {
            var repositorio = new EmpleadoRepositorio();
            _empleadoService = new EmpleadoService(repositorio);
        }

        // GET: Empleado
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult ListarEmpleado()
        {
            try
            {
                List<Empleados> lista = _empleadoService.ListarEmpleado();
                return Json(new { success = true, data = lista }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult GuardarEmpleado(Empleados empleados)
        {
            object respuesta;
            string mensaje = string.Empty;

            try
            {
                if (empleados.IdEmpleado == 0)
                    respuesta = _empleadoService.Crear(empleados, out mensaje);
                else
                    respuesta = _empleadoService.Actualizar(empleados, out mensaje);
            }
            catch (Exception ex)
            {
                return Json(new { respuesta = false, mensaje = ex.Message }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { respuesta, message = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EliminarEmpleado(int id)
        {
            try
            {
                string mensaje;
                bool resultado = _empleadoService.Eliminar(id, out mensaje);
                return Json(new { success = resultado, message = mensaje }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error en el servidor, contactar al administrador.", ex.Message });
            }
        }
    }
}