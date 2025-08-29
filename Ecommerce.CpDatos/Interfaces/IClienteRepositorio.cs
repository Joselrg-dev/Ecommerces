using Ecommerce.CpEntities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.CpDatos.Interfaces
{
    public interface IClienteRepositorio
    {
        Clientes ObtenerPorId(int id);

        List<Clientes> ObtenerTodo();

        int Insertar(Clientes clientes, out string mensaje);

        bool Actualizar(Clientes clientes, out string mensaje);

        bool Eliminar(int id, out string mensaje);

        bool CambiarContraseña(int idCliente, string nuevaContra, out string mensaje);

        bool ReestablecerContraseña(int idCliente, string nuevaContra, out string mensaje);
    }
}
