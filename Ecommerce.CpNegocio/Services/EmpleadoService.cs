using Ecommerce.CpCommons;
using Ecommerce.CpDatos.Interfaces;
using Ecommerce.CpEntities.Models;
using Ecommerce.CpNegocio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.CpNegocio.Services
{
    public class EmpleadoService : IEmpleadoService
    {
        private readonly IEmpleadoRepositorio _empleadoRepo;

        public EmpleadoService(IEmpleadoRepositorio empleadoRepositorio)
        {
            _empleadoRepo = empleadoRepositorio;
        }

        public bool Actualizar(Empleados empleados, out string mensaje)
        {
            mensaje = string.Empty;

            if (empleados.IdEmpleado <= 0)
                throw new ArgumentException("El empleado debe tener un Id válido");

            if (string.IsNullOrEmpty(empleados.CodigoEmpleado) || string.IsNullOrWhiteSpace(empleados.CodigoEmpleado))
                mensaje = ("El campo código del empleado es requerido.");
            else if (string.IsNullOrEmpty(empleados.NombreEmpleado) || string.IsNullOrWhiteSpace(empleados.NombreEmpleado))
                mensaje = "El campo nombre es requerido.";
            else if (string.IsNullOrEmpty(empleados.Apellido1Empleado) || string.IsNullOrWhiteSpace(empleados.Apellido1Empleado))
                mensaje = "El campo primer apellido es requerido.";
            else if (string.IsNullOrEmpty(empleados.CorreoEmpleado) || string.IsNullOrWhiteSpace(empleados.CorreoEmpleado))
                mensaje = "El campo correo es requerido";

            if (string.IsNullOrEmpty(mensaje))
                return _empleadoRepo.Actualizar(empleados, out mensaje);
            else
                return false;
        }

        public int Crear(Empleados empleados, out string mensaje)
        {
            mensaje = string.Empty;

            if (string.IsNullOrEmpty(empleados.CodigoEmpleado) || string.IsNullOrWhiteSpace(empleados.CodigoEmpleado))
                mensaje = ("El campo código del empleado es requerido.");
            else if (string.IsNullOrEmpty(empleados.NombreEmpleado) || string.IsNullOrWhiteSpace(empleados.NombreEmpleado))
                mensaje = "El campo nombre es requerido.";
            else if (string.IsNullOrEmpty(empleados.Apellido1Empleado) || string.IsNullOrWhiteSpace(empleados.Apellido1Empleado))
                mensaje = "El campo primer apellido es requerido.";
            else if (string.IsNullOrEmpty(empleados.CorreoEmpleado) || string.IsNullOrWhiteSpace(empleados.CorreoEmpleado))
                mensaje = "El campo correo es requerido";

            if (!string.IsNullOrEmpty(mensaje))
                return 0;

            string clave = SeguridadHelpers.GenerarClave();
            string asunto = "¡Bienvenido/a al sistema!";
            string cuerpoMensaje = @"
                <p>Estimad@ usuario/a,</p>
                <p>Su cuenta ha sido creada exitosamente.</p>
                <p>Sus credenciales de acceso son las siguientes:</p>
                <ul>
                    <li><strong>Contraseña temporal:</strong> !clave!</li>
                </ul>
                <p>Le recomendamos cambiar esta contraseña una vez haya iniciado sesión por primera vez para mayor seguridad.</p>
                <br>
                <p>Si usted no solicitó esta cuenta, por favor comuníquese con el equipo de soporte.</p>
                <br>
                <p>Atentamente,<br>El equipo de soporte</p>";
            cuerpoMensaje = cuerpoMensaje.Replace("!clave!", clave);

            bool correoEnviado = SeguridadHelpers.EnviarCorreo(empleados.CorreoEmpleado, asunto, cuerpoMensaje);

            if (correoEnviado)
            {
                empleados.Clave = SeguridadHelpers.GetSHA256(clave);
                return _empleadoRepo.Insertar(empleados, out mensaje);
            }
            else
            {
                mensaje = "No se pudo enviar el correo de bienvenida al empleado.";
                return 0;
            }
        }

        public bool Eliminar(int id, out string mensaje)
        {
            mensaje = string.Empty;
            if (id <= 0)
                throw new ArgumentException("Id invalido para eliminar");
            else
                return _empleadoRepo.Eliminar(id, out mensaje);
        }

        public List<Empleados> ListarEmpleado()
        {
            return _empleadoRepo.ObtenerTodos();
        }

        public Empleados ObtenerPorId(int id)
        {
            throw new NotImplementedException();
        }
    }
}
