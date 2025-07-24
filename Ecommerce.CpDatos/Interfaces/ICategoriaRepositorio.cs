
using Ecommerce.CpEntities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.CpDatos.Interfaces
{
    public interface ICategoriaRepositorio
    {
        Categorias ObtenerPorId(int id);

        List<Categorias> Listar();

        int Insertar(Categorias categorias, out string mensaje);

        bool Actualizar(Categorias categorias, out string mensaje);

        bool Eliminar(int id, out string mensaje);
    }
}
