using Ecommerce.CpCommons;
using Ecommerce.CpDatos.Repositorio;
using Ecommerce.CpEntities.Models;
using Ecommerce.CpNegocio.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ecommerce.WebApplication.Controllers
{
    public class AccesoController : Controller
    {
        // GET: Acceso
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CambiarClave()
        {
            return View();
        }

        public ActionResult ReestablecerContra()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string email, string password)
        {
            var objEmpleado = new Empleados();
            objEmpleado = new EmpleadoService().ListarEmpleado().Where(user => user.CorreoEmpleado == email
                                                                            && user.Clave == SeguridadHelpers.GetSHA256(password)).FirstOrDefault();

            if (objEmpleado == null)
            {
                ViewBag.Error = "Correo o Contraseña invalida";
                return View();
            }
            else
            {

                if ((bool)objEmpleado.Reestablecer)
                {
                    TempData["IdEmpleado"] = objEmpleado.IdEmpleado;
                    return RedirectToAction("CambiarClave");
                }

                ViewBag.Error = null;
                return RedirectToAction("Index", "Dashboard");
            }
        }

        [HttpPost]
        public ActionResult CambiarClave(string idEmpleado, string actualClave, string nuevaClave, string confirmarClave)
        {
            Empleados objEmpleado = new Empleados();

            objEmpleado = new EmpleadoRepositorio().ObtenerTodos().Where(emp => emp.IdEmpleado == int.Parse(idEmpleado)).FirstOrDefault();

            TempData["IdEmpleado"] = idEmpleado;

            if (objEmpleado.Clave != SeguridadHelpers.GetSHA256(actualClave))
            {
                ViewData["viewClave"] = "";
                ViewBag.Error = "La contraseña actual no es correcta";
                return View();
            }
            else if(nuevaClave != confirmarClave )
            {
                TempData["IdEmpleado"] = idEmpleado;
                ViewData["viewClave"] = actualClave;
                ViewBag.Error = "Las contraseñas no coinciden";
                return View();
            }

            ViewData["viewClave"] = "";

            nuevaClave = SeguridadHelpers.GetSHA256(nuevaClave);

            string mensaje = string.Empty;

            bool respuesta = new EmpleadoService().CambiarContraseña(int.Parse(idEmpleado), nuevaClave, out mensaje);

            if (respuesta)
                return RedirectToAction("Index");
            else
            {
                TempData["IdEmpleado"] = idEmpleado;
                ViewBag.Error = mensaje;
                return View();
            }
        }
    }
}