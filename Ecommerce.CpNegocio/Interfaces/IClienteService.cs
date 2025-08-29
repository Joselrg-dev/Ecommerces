using Ecommerce.CpEntities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.CpNegocio.Interfaces
{
    interface IClienteService
    {
        Clientes ObtenerPorId(int id);

        List<Clientes> ListarClientes();

        int Crear(Clientes clientes, out string mensaje);

        bool Actualizar(Clientes clientes, out string mensaje);

        bool Eliminar(int id, out string mensaje);

        bool CambiarContraseña(int idCliente, string correo, out string mensaje);

        bool ReestablecerContraseña(int idCliente, string correo, out string mensaje);
    }
}
