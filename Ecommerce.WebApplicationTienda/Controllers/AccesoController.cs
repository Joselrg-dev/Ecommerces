using Ecommerce.CpCommons;
using Ecommerce.CpDatos.Repositorio;
using Ecommerce.CpEntities.Models;
using Ecommerce.CpNegocio.Interfaces;
using Ecommerce.CpNegocio.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Ecommerce.WebApplicationTienda.Controllers
{
    public class AccesoController : Controller
    {

        private readonly IClienteService _clienteService;

        public AccesoController()
        {

            var repo = new ClienteRepositorio();
            _clienteService = new ClienteService(repo);
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Reestablecer()
        {
            return View();
        }

        public ActionResult CambiarClave()
        {
            return View();
        }

        public ActionResult Registrar()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Registrar(Clientes objClientes)
        {
            int resultado;
            string mensaje = string.Empty;

            ViewData["Nombres"] = string.IsNullOrEmpty(objClientes.NombreCliente) ? "" : objClientes.NombreCliente;
            ViewData["Apellido1Cliente"] = string.IsNullOrEmpty(objClientes.Apellido1Cliente) ? "" : objClientes.Apellido1Cliente;
            ViewData["Apellido2Cliente"] = string.IsNullOrEmpty(objClientes.Apellido2Cliente) ? "" : objClientes.Apellido2Cliente;
            ViewData["CorreoCliente"] = string.IsNullOrEmpty(objClientes.CorreoCliente) ? "" : objClientes.CorreoCliente;
            ViewData["TelefonoCliente"] = string.IsNullOrEmpty(objClientes.TelefonoCliente) ? "" : objClientes.TelefonoCliente;

            if (objClientes.Clave != objClientes.ConfirmarClave)
            {
                ViewBag.Error = "Las contraseñas no coinciden";
                return View();
            }

            resultado = _clienteService.Crear(objClientes, out mensaje);

            if(resultado > 0)
            {
                ViewBag.Error = null;
                return RedirectToAction("Index", "Acceso");
            }
            else
            {
                ViewBag.Error = mensaje;
                return View();
            }
        }

        [HttpPost]
        public ActionResult Index(string email, string password)
        {
            Clientes objCliente = null;
            string contraEncriptada = SeguridadHelpers.GetSHA256(password);

            objCliente = _clienteService.ListarClientes().Where(item => item.CorreoCliente == email && item.Clave == contraEncriptada).FirstOrDefault();

            if(objCliente == null)
            {
                ViewBag.Error = "Correo o contraseña incorrectas";
                return View();
            }
            else
            {
                if ((bool)objCliente.Reestablecer)
                {
                    TempData["IdEmpleado"] = objCliente.IdCliente;
                    return RedirectToAction("CambiarClave", "Acceso");
                }
                else
                {
                    FormsAuthentication.SetAuthCookie(objCliente.CorreoCliente, false);
                    Session["Cliente"] = objCliente;
                    
                    ViewBag.Error = null;
                    return RedirectToAction("Index", "Tienda");
                }
            }
        }

        [HttpPost]
        public ActionResult Reestablecer(string email)
        {
            var objCliente = new Clientes();
            objCliente = _clienteService.ListarClientes().Where(item => item.CorreoCliente == email).FirstOrDefault();

            if (string.IsNullOrWhiteSpace(email))
            {
                ViewBag.Error = "Debes ingresar un correo válido.";
                return View();
            }

            if (objCliente == null)
            {
                ViewBag.Error = "No se ha encontrado un usuario relacionado con dicho correo!";
                return View();
            }

            // Aquí iría la lógica de envío de correo
            string mensaje = string.Empty;
            bool respuesta = _clienteService.ReestablecerContraseña(objCliente.IdCliente, email, out mensaje);

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

        [HttpPost]
        public ActionResult CambiarClave(string idCliente, string currentPassword, string newPassword, string confirmPassword)
        {
            if (!int.TryParse(idCliente, out int clienteId))
            {
                ViewBag.Error = "El Id del cliente no es válido.";
                return View();
            }

            var cliente = _clienteService.ListarClientes()
                .Where(item => item.IdCliente == int.Parse(idCliente))
                .FirstOrDefault();

            if (cliente == null)
            {
                ViewBag.Error = "Cliente no encontrado.";
                return View();
            }

            // Validación de clave actual
            if (cliente.Clave != SeguridadHelpers.GetSHA256(currentPassword))
            {
                TempData["IdCliente"] = idCliente;
                ViewData["viewClave"] = "";
                ViewBag.Error = "La contraseña actual no es correcta.";
                return View();
            }
            else if (newPassword != confirmPassword)
            {
                TempData["IdCliente"] = idCliente;
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
            if (cliente.Clave == nuevaClaveHash)
            {
                ViewBag.Error = "La nueva contraseña no puede ser igual a la anterior.";
                return View();
            }

            bool resultado = _clienteService.CambiarContraseña(clienteId, nuevaClaveHash, out string mensaje);

            if (!resultado)
            {
                ViewBag.Error = mensaje ?? "No fue posible cambiar la contraseña.";
                return View();
            }

            // Si todo salió bien → redirigir al login
            TempData["Success"] = "Tu contraseña se cambió correctamente. Vuelve a iniciar sesión.";
            return RedirectToAction("Index", "Acceso");
        }
    }
}