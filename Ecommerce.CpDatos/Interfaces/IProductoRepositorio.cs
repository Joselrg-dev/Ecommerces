using Ecommerce.CpEntities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.CpDatos.Interfaces
{
    public interface IProductoRepositorio
    {
        Productos ObtenerPorId(int id);

        List<Productos> Listar();

        int Insertar(Productos productos, out string mensaje);

        bool Actualizar(Productos productos, out string mensaje);

        bool Eliminar(int id, out string mensaje);

        bool GuardarImagen(Productos productos, out string mensaje);
    }
}
