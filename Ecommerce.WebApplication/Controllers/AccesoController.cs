using Ecommerce.CpCommons;
using Ecommerce.CpDatos.Repositorio;
using Ecommerce.CpEntities.Models;
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

        public ActionResult CambiarContra()
        {
            return View();
        }

        public ActionResult ReestablecerContra()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string correo, string clave)
        {
            var objEmpleado = new Empleados();
            objEmpleado = new EmpleadoRepositorio().ObtenerTodos().Where(user => user.CorreoEmpleado == correo
                                                                            && user.Clave == SeguridadHelpers.GetSHA256(clave)).FirstOrDefault();

            if (objEmpleado == null)
            {
                ViewBag.Error = "Correo o Contraseña invalida";
                return View();
            }
            else
            {

                //if ((bool)objEmpleado.Reestablecer)
                //{
                //    TempData["Id"] = objEmpleado.Id;
                //    return RedirectToAction("CambiarContra");
                //}

                ViewBag.Error = null;
                return RedirectToAction("Index", "Dashboard");
            }
        }
    }
}