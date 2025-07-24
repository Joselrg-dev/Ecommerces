using Ecommerce.CpEntities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.CpNegocio.Interfaces
{
    public interface ICategoriaService
    {
        Categorias ObtenerPorId(int id);

        List<Categorias> ListarCategorias();

        int Crear(Categorias categorias, out string mensaje);

        bool Actualizar(Categorias categorias, out string mensaje);

        bool Eliminar(int id, out string mensaje);
    }
}
