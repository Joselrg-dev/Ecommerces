using Ecommerce.CpCommons;
using Ecommerce.CpDatos.Interfaces;
using Ecommerce.CpDatos.Repositorio;
using Ecommerce.CpEntities.Models;
using Ecommerce.CpNegocio.Interfaces;
using System;
using System.Collections.Generic;

namespace Ecommerce.CpNegocio.Services
{
    /// <summary>
    /// Servicio de negocio para la gestión de empleados.
    /// Implementa la interfaz IEmpleadoService y actúa como intermediario entre la capa de presentación y el repositorio.
    /// </summary>
    public class EmpleadoService : IEmpleadoService
    {
        private readonly IEmpleadoRepositorio _empleadoRepo;

        /// <summary>
        /// Constructor obligatorio con inyección de dependencias.
        /// </summary>
        /// <param name="empleadoRepositorio">Repositorio de empleados</param>
        public EmpleadoService(IEmpleadoRepositorio empleadoRepositorio)
        {
            _empleadoRepo = empleadoRepositorio ?? throw new ArgumentNullException(nameof(empleadoRepositorio));
        }

        #region CRUD

        public int Crear(Empleados empleado, out string mensaje)
        {
            mensaje = string.Empty;
            if (!ValidarEmpleado(empleado, out mensaje)) return 0;

            string claveTemporal = SeguridadHelpers.GenerarClave();
            string asunto = "¡Bienvenido/a al sistema!";
            string cuerpoMensaje = $@"
                <p>Estimad@ usuario/a,</p>
                <p>Su cuenta ha sido creada exitosamente.</p>
                <p>Sus credenciales de acceso son las siguientes:</p>
                <ul>
                    <li><strong>Contraseña temporal:</strong> {claveTemporal}</li>
                </ul>
                <p>Le recomendamos cambiar esta contraseña una vez haya iniciado sesión por primera vez.</p>
                <br>
                <p>Si usted no solicitó esta cuenta, por favor comuníquese con soporte.</p>
                <br>
                <p>Atentamente,<br>El equipo de soporte</p>";

            if (SeguridadHelpers.EnviarCorreo(empleado.CorreoEmpleado, asunto, cuerpoMensaje))
            {
                empleado.Clave = SeguridadHelpers.GetSHA256(claveTemporal);
                return _empleadoRepo.Insertar(empleado, out mensaje);
            }

            mensaje = "No se pudo enviar el correo de bienvenida al empleado.";
            return 0;
        }

        public bool Actualizar(Empleados empleado, out string mensaje)
        {
            mensaje = string.Empty;

            if (empleado.IdEmpleado <= 0)
                throw new ArgumentException("El empleado debe tener un Id válido");

            if (!ValidarEmpleado(empleado, out mensaje)) return false;

            return _empleadoRepo.Actualizar(empleado, out mensaje);
        }

        public bool Eliminar(int id, out string mensaje)
        {
            if (id <= 0)
                throw new ArgumentException("Id inválido para eliminar");

            return _empleadoRepo.Eliminar(id, out mensaje);
        }

        public List<Empleados> ListarEmpleado()
        {
            return _empleadoRepo.ObtenerTodos();
        }

        public Empleados ObtenerPorId(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Id inválido para obtener empleado");

            return _empleadoRepo.ObtenerPorId(id);
        }

        #endregion

        #region Gestión de Contraseñas

        public bool CambiarContraseña(int idUsuario, string clave, out string mensaje)
        {
            if (idUsuario <= 0)
                throw new ArgumentException("Id inválido para realizar acción.");

            return _empleadoRepo.CambiarContraseña(idUsuario, clave, out mensaje);
        }

        public bool ReestablecerContraseña(int idUsuario, string correo, out string mensaje)
        {
            mensaje = string.Empty;
            string nuevaClave = SeguridadHelpers.GenerarClave();

            if (!_empleadoRepo.ReestablecerContraseña(idUsuario, SeguridadHelpers.GetSHA256(nuevaClave), out mensaje))
            {
                mensaje = "¡Hubo un error al reestablecer la contraseña!";
                return false;
            }

            string asunto = "<strong>Restablecimiento de Contraseña Exitoso</strong>";
            string cuerpoMensaje = $@"
                <h3>Estimado/a usuario/a,</h3>
                <p>Su contraseña ha sido restablecida correctamente.</p>
                <p>Su nueva contraseña es: <strong>{nuevaClave}</strong></p>
                <p>Por razones de seguridad, le recomendamos cambiarla después de iniciar sesión.</p>
                <br>
                <p>Si usted no solicitó este cambio, contacte al administrador.</p>
                <br>
                <p>Atentamente,<br>El equipo de soporte</p>";

            if (SeguridadHelpers.EnviarCorreo(correo, asunto, cuerpoMensaje))
            {
                mensaje = "Correo enviado exitosamente";
                return true;
            }

            mensaje = "¡Hubo un error al enviar el correo!";
            return false;
        }

        #endregion

        #region Métodos Privados

        private bool ValidarEmpleado(Empleados empleado, out string mensaje)
        {
            mensaje = string.Empty;

            if (string.IsNullOrWhiteSpace(empleado.CodigoEmpleado))
                mensaje = "El campo código del empleado es requerido.";
            else if (string.IsNullOrWhiteSpace(empleado.NombreEmpleado))
                mensaje = "El campo nombre es requerido.";
            else if (string.IsNullOrWhiteSpace(empleado.Apellido1Empleado))
                mensaje = "El campo primer apellido es requerido.";
            else if (string.IsNullOrWhiteSpace(empleado.CorreoEmpleado))
                mensaje = "El campo correo es requerido.";

            return string.IsNullOrEmpty(mensaje);
        }

        #endregion
    }
}
