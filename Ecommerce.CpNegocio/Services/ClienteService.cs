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
    public class ClienteService : IClienteService
    {
        private readonly IClienteRepositorio _clienteRepo;

        /// <summary>
        /// Constructor obligatorio con inyección de dependencias.
        /// </summary>
        /// <param name="clienteRepositorio">Repositorio de clientes</param>
        public ClienteService(IClienteRepositorio clienteRepositorio)
        {
            _clienteRepo = clienteRepositorio ?? throw new ArgumentNullException(nameof(clienteRepositorio));
        }

        public bool Actualizar(Clientes clientes, out string mensaje)
        {
            mensaje = string.Empty;

            if (clientes.IdCliente <= 0)
                throw new ArgumentException("El cliente debe tener un Id válido");

            if (!ValidarCliente(clientes, out mensaje)) return false;

            return _clienteRepo.Actualizar(clientes, out mensaje);
        }

        public bool CambiarContraseña(int idCliente, string correo, out string mensaje)
        {
            if (idCliente <= 0)
                throw new ArgumentException("Id inválido para realizar acción.");

            return _clienteRepo.CambiarContraseña(idCliente, correo, out mensaje);
        }

        public int Crear(Clientes clientes, out string mensaje)
        {
            mensaje = string.Empty;
            if (!ValidarCliente(clientes, out mensaje))
            {
                clientes.Clave = SeguridadHelpers.GetSHA256(clientes.Clave);
                return _clienteRepo.Insertar(clientes, out mensaje);
            }
            else
            {
                mensaje = "Error: Cliente no registrado.";
                return 0;
            }
        }

        public bool Eliminar(int id, out string mensaje)
        {
            if (id <= 0)
                throw new ArgumentException("Id inválido para eliminar");

            return _clienteRepo.Eliminar(id, out mensaje);
        }

        public List<Clientes> ListarClientes()
        {
            return _clienteRepo.ObtenerTodo();
        }

        public Clientes ObtenerPorId(int id)
        {
            throw new NotImplementedException();
        }

        public bool ReestablecerContraseña(int idCliente, string correo, out string mensaje)
        {
            mensaje = string.Empty;
            string nuevaClave = SeguridadHelpers.GenerarClave();

            if (!_clienteRepo.ReestablecerContraseña(idCliente, SeguridadHelpers.GetSHA256(nuevaClave), out mensaje))
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

        private bool ValidarCliente(Clientes cliente, out string mensaje)
        {
            mensaje = string.Empty;
            
            if (string.IsNullOrWhiteSpace(cliente.NombreCliente))
                mensaje = "El campo nombre es requerido.";
            else if (string.IsNullOrWhiteSpace(cliente.Apellido1Cliente))
                mensaje = "El campo primer apellido es requerido.";
            else if (string.IsNullOrWhiteSpace(cliente.CorreoCliente))
                mensaje = "El campo correo es requerido.";

            return string.IsNullOrEmpty(mensaje);
        }
    }
}
