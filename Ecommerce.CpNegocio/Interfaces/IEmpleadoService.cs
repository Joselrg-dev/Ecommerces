using Ecommerce.CpEntities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.CpNegocio.Interfaces
{
    public interface IEmpleadoService
    {
        Empleados ObtenerPorId(int id);

        List<Empleados> ListarEmpleado();

        int Crear(Empleados empleados, out string mensaje);

        bool Actualizar(Empleados empleados, out string mensaje);

        bool Eliminar(int id, out string mensaje);

        bool CambiarContraseña(int idUsuario, string correo, out string mensaje);

        bool ReestablecerContraseña(int idUsuario, string correo, out string mensaje);

    }
}
