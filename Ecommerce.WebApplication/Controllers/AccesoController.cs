using Ecommerce.CpCommons;
using Ecommerce.CpDatos.Repositorio;
using Ecommerce.CpEntities.Models;
using Ecommerce.CpNegocio.Services;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Ecommerce.WebApplication.Controllers
{
    /// <summary>
    /// Controlador encargado de gestionar la autenticación y administración de contraseñas
    /// para los empleados del sistema de e-commerce.
    /// </summary>
    public class AccesoController : Controller
    {
        #region Vistas principales

        /// <summary>
        /// Muestra la vista de login donde el usuario ingresa sus credenciales.
        /// </summary>
        /// <returns>Vista de login</returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Vista para el cambio de contraseña (usado cuando se fuerza al empleado a reestablecerla).
        /// </summary>
        /// <returns>Vista para cambiar contraseña</returns>
        public ActionResult CambiarClave()
        {
            return View();
        }

        /// <summary>
        /// Vista para reestablecer contraseña olvidada (flujo futuro).
        /// </summary>
        /// <returns>Vista de reestablecer contraseña</returns>
        public ActionResult ReestablecerContra()
        {
            return View();
        }

        #endregion

        #region Autenticación

        /// <summary>
        /// Acción que valida las credenciales de un empleado contra la base de datos.
        /// </summary>
        /// <param name="email">Correo del empleado</param>
        /// <param name="password">Contraseña sin encriptar, que será validada con hash SHA256</param>
        /// <returns>
        /// - Redirección al Dashboard si es válido.  
        /// - Redirección a CambiarClave si se requiere reestablecimiento.  
        /// - Vista de login con error en caso contrario.
        /// </returns>
        [HttpPost]
        public ActionResult Index(string email, string password)
        {
            // Buscar empleado con correo y contraseña encriptada
            var objEmpleado = new EmpleadoService().ListarEmpleado()
                .FirstOrDefault(user => user.CorreoEmpleado == email
                                     && user.Clave == SeguridadHelpers.GetSHA256(password));

            if (objEmpleado == null)
            {
                // Credenciales inválidas → se notifica en la vista
                ViewBag.Error = "Correo o Contraseña inválida";
                return View();
            }
            else
            {
                // Si el empleado tiene flag de reestablecer, lo mandamos a cambiar clave
                if ((bool)objEmpleado.Reestablecer)
                {
                    TempData["IdEmpleado"] = objEmpleado.IdEmpleado;
                    return RedirectToAction("CambiarClave");
                }

                // Autenticación exitosa → Dashboard
                ViewBag.Error = null;
                return RedirectToAction("Index", "Dashboard");
            }
        }

        #endregion

        #region Gestión de contraseñas

        /// <summary>
        /// Cambia la contraseña de un empleado, validando la actual y verificando que
        /// la nueva contraseña coincida con la confirmación.
        /// </summary>
        /// <param name="idEmpleado">Id del empleado que solicita el cambio</param>
        /// <param name="actualClave">Contraseña actual ingresada</param>
        /// <param name="nuevaClave">Nueva contraseña propuesta</param>
        /// <param name="confirmarClave">Confirmación de la nueva contraseña</param>
        /// <returns>
        /// - Redirección al login si se cambió correctamente.  
        /// - Misma vista con errores si falló la validación o actualización.
        /// </returns>
        [HttpPost]
        public ActionResult CambiarClave(string idEmpleado, string actualClave, string nuevaClave, string confirmarClave)
        {
            // Recuperar empleado desde repositorio
            var objEmpleado = new EmpleadoRepositorio()
                                .ObtenerTodos()
                                .FirstOrDefault(emp => emp.IdEmpleado == int.Parse(idEmpleado));

            // Mantener el idEmpleado en el ciclo de vida de la vista
            TempData["IdEmpleado"] = idEmpleado;

            // Validación: contraseña actual correcta
            if (objEmpleado.Clave != SeguridadHelpers.GetSHA256(actualClave))
            {
                ViewBag.Error = "La contraseña actual no es correcta";
                return View();
            }

            // Validación: nueva y confirmación coinciden
            if (nuevaClave != confirmarClave)
            {
                ViewBag.Error = "Las contraseñas no coinciden";
                return View();
            }

            // Hash de la nueva clave antes de guardarla en BD
            nuevaClave = SeguridadHelpers.GetSHA256(nuevaClave);

            string mensaje = string.Empty;

            // Se actualiza la contraseña a través del servicio
            bool respuesta = new EmpleadoService().CambiarContraseña(int.Parse(idEmpleado), nuevaClave, out mensaje);

            if (respuesta)
                return RedirectToAction("Index"); // Éxito → volver al login
            else
            {
                // Si falla, mostrar mensaje de error
                ViewBag.Error = mensaje;
                return View();
            }
        }

        #endregion
    }
}
