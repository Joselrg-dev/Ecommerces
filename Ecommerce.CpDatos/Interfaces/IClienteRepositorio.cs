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

        void Insertar(Clientes clientes);

        void Actualizar(Clientes clientes);

        void Eliminar(int id);
    }
}
