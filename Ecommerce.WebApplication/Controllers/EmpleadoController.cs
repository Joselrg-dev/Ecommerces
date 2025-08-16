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
    /// <summary>
    /// Controlador encargado de gestionar la administración de empleados del e-commerce.
    /// Incluye operaciones CRUD.
    /// </summary>
    public class EmpleadoController : Controller
    {
        private readonly IEmpleadoService _empleadoService;


        /// <summary>
        /// Inicializa el controlador, configurando el servicio de empleado
        /// </summary>
        public EmpleadoController()
        {
            var repositorio = new EmpleadoRepositorio();
            _empleadoService = new EmpleadoService(repositorio);
        }

        #region Vista

        /// <summary>
        /// Muestra la vista principal de gestión de empleados.
        /// </summary>
        /// <returns></returns>
        // GET: Empleado
        public ActionResult Index()
        {
            return View();
        }

        #endregion

        #region Metodos - CRUD Empleado
        /// <summary>
        /// Obtiene la lista de empleados en formato JSON.
        /// </summary>
        /// <returns>
        /// JSON con:
        /// - success: indica exito o fallo
        /// - data: lista de empleados
        /// - message: error si aplica
        /// </returns>

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

        /// <summary>
        /// Crea o actualiza un empleado, validando de acuerdo al IdEmpleado
        /// </summary>
        /// <param name="empleados">JSON serializado de un objeto Empleado</param>
        /// <returns>
        /// JSON con:
        /// - respuesta: indica éxito o fallo
        /// - message: mensajes adicionales o errores
        /// </returns>

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

        /// <summary>
        /// Elimina un Empleado según su Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

        #endregion

    }
}