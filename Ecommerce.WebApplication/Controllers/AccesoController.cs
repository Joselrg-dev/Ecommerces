using Ecommerce.CpCommons;
using Ecommerce.CpDatos;
using Ecommerce.CpDatos.Repositorio;
using Ecommerce.CpEntities.Models;
using Ecommerce.CpNegocio;
using Ecommerce.CpNegocio.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Ecommerce.WebApplication.Controllers
{
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
        /// Acción que valida las credenciales de un 
        /// empleado contra la base de datos. 
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
            try
            {
                var repo = new EmpleadoRepositorio();
                var empleadoService = new EmpleadoService(repo);
                var objEmpleado = empleadoService.ListarEmpleado()
                    .Where(user => user.CorreoEmpleado == email
                                         && user.Clave == SeguridadHelpers.GetSHA256(password)).FirstOrDefault();

                if (objEmpleado == null)
                {
                    ViewBag.Error = "Usuario o clave incorrectos.";
                    return View();
                }
                else
                {
                    if ((bool)objEmpleado.Reestablecer)
                    {
                        TempData["IdEmpleado"] = objEmpleado.IdEmpleado;
                        return RedirectToAction("CambiarClave");
                    }

                    FormsAuthentication.SetAuthCookie(objEmpleado.CorreoEmpleado, false);

                    Session["Empleado"] = objEmpleado; // guarda sesión
                    return RedirectToAction("Index", "Dashboard");
                }

            }
            catch (Exception ex)
            {
                ViewBag.Error = "Ocurrió un error inesperado. Intenta de nuevo." + ex.Message;
                return View();
            }
        }

        #endregion

        #region Gestión de contraseñas

        /// <summary>
        /// Cambia la contraseña de un empleado, validando la actual y verificando que
        /// la nueva contraseña coincida con la confirmación.
        /// </summary>
        /// <param name="idEmpleado">Id del empleado que solicita el cambio</param>
        /// <param name="currentPassword">Contraseña actual ingresada</param>
        /// <param name="newPassword">Nueva contraseña propuesta</param>
        /// <param name="confirmPassword">Confirmación de la nueva contraseña</param>
        /// <returns>
        /// - Redirección al login si se cambió correctamente.  
        /// - Misma vista con errores si falló la validación o actualización.
        /// </returns>
        [HttpPost]
        public ActionResult CambiarClave(string idEmpleado, string currentPassword, string newPassword, string confirmPassword)
        {
            if (!int.TryParse(idEmpleado, out int empleadoId))
            {
                ViewBag.Error = "El Id de empleado no es válido.";
                return View();
            }

            var repo = new EmpleadoRepositorio();
            var empleadoService = new EmpleadoService(repo);
            var empleado = empleadoService.ObtenerPorId(empleadoId);


            if (empleado == null)
            {
                ViewBag.Error = "Empleado no encontrado.";
                return View();
            }

            // Validación de clave actual
            if (empleado.Clave != SeguridadHelpers.GetSHA256(currentPassword))
            {
                TempData["IdEmpleado"] = idEmpleado;
                ViewData["viewClave"] = "";
                ViewBag.Error = "La contraseña actual no es correcta.";
                return View();
            }
            else if (newPassword != confirmPassword)
            {
                TempData["IdEmpleado"] = idEmpleado;
                ViewData["viewClave"] = currentPassword;
                ViewBag.Error = "Las contraseñas no coinciden.";
                return View();
            }
            ViewData["viewClave"] = "";

            // Validación mínima de seguridad
            if (newPassword.Length <= 8)
            {
                ViewBag.Error = "La nueva contraseña debe tener al menos 8 caracteres.";
                return View();
            }

            // Hash + persistencia
            var nuevaClaveHash = SeguridadHelpers.GetSHA256(newPassword);
            if (empleado.Clave == nuevaClaveHash)
            {
                ViewBag.Error = "La nueva contraseña no puede ser igual a la anterior.";
                return View();
            }

            bool resultado = empleadoService.CambiarContraseña(empleadoId, nuevaClaveHash, out string mensaje);

            if (!resultado)
            {
                ViewBag.Error = mensaje ?? "No fue posible cambiar la contraseña.";
                return View();
            }

            // Si todo salió bien → redirigir al login
            TempData["Success"] = "Tu contraseña se cambió correctamente. Vuelve a iniciar sesión.";
            return RedirectToAction("Index", "Acceso");
        }

        [HttpPost]
        public ActionResult Reestablecer(string email)
        {
            var objUsuario = new Empleados();
            var repo = new EmpleadoRepositorio();
            var empleadoService = new EmpleadoService(repo);
            objUsuario = empleadoService.ListarEmpleado().Where(item => item.CorreoEmpleado == email).FirstOrDefault();

            if (string.IsNullOrWhiteSpace(email))
            {
                ViewBag.Error = "Debes ingresar un correo válido.";
                return View();
            }

            if (objUsuario == null)
            {
                ViewBag.Error = "No se ha encontrado un usuario relacionado con dicho correo!";
                return View();
            }

            // Aquí iría la lógica de envío de correo
            string mensaje = string.Empty;
            bool respuesta = empleadoService.ReestablecerContraseña(objUsuario.IdEmpleado, email, out mensaje);

            if (respuesta)
            {
                TempData["Success"] = "Si el correo existe en nuestro sistema, recibirás un enlace para reestablecer tu contraseña.";
                return RedirectToAction("Index", "Acceso");
            }
            else
            {
                ViewBag.Error = mensaje;
                return View();
            }
        }

        #endregion

        public ActionResult CerrarSesion()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Acceso");
        }
    }
}
