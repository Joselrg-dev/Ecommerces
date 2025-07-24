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
    public class RolServices : IRolServices
    {
        private readonly IRolRepositorio _rolRepositorio;

        public RolServices(IRolRepositorio rolRepositorio)
        {
            _rolRepositorio = rolRepositorio;
        }

        public bool Actualizar(Roles roles, out string mensaje)
        {
            mensaje = string.Empty;

            if (roles.IdRol <= 0)
                throw new ArgumentException("El rol debe tener un Id válido");

            if (string.IsNullOrEmpty(roles.NombreRol) || string.IsNullOrWhiteSpace(roles.NombreRol))
                mensaje = ("El campo nombre del rol es requerido.");

            if (string.IsNullOrEmpty(mensaje))
                return _rolRepositorio.Actualizar(roles, out mensaje);
            else
                return false;
        }

        public int Crear(Roles roles, out string mensaje)
        {
            mensaje = string.Empty;

            if (string.IsNullOrEmpty(roles.NombreRol) || string.IsNullOrWhiteSpace(roles.NombreRol))
                mensaje = ("El campo nombre del rol es requerido.");

            if (string.IsNullOrEmpty(mensaje))
                return _rolRepositorio.Insertar(roles, out mensaje);
            else
                return 0;
        }

        public bool Eliminar(int id, out string mensaje)
        {
            mensaje = string.Empty;
            if (id <= 0)
                throw new ArgumentException("Id invalido para eliminar");
            else
                return _rolRepositorio.Eliminar(id, out mensaje);
        }

        public List<Roles> ListarTodo()
        {
            return _rolRepositorio.Listar();
        }

        public Roles ObtenerPorId(int id)
        {
            throw new NotImplementedException();
        }
    }
}
