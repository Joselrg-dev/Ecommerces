using Ecommerce.CpEntities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.CpNegocio.Interfaces
{
    public interface IProductoService
    {
        Productos ObtenerPorId(int id);

        List<Productos> ListarProducto();

        int Crear(Productos productos, out string mensaje);

        bool Actualizar(Productos productos, out string mensaje);

        bool Eliminar(int id, out string mensaje);

        bool GuardarImagen(Productos productos, out string mensaje);
    }
}
