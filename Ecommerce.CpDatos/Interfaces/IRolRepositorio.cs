using Ecommerce.CpEntities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.CpDatos.Interfaces
{
    public interface IRolRepositorio
    {
        Roles ObtenerPorId(int id);

        List<Roles> Listar();

        int Insertar(Roles roles, out string mensaje);

        bool Actualizar(Roles roles, out string mensaje);

        bool Eliminar(int id, out string mensaje);
    }
}
