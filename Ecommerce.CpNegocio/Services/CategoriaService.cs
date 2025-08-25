using Ecommerce.CpDatos.Interfaces;
using Ecommerce.CpDatos.Repositorio;
using Ecommerce.CpEntities.Models;
using Ecommerce.CpNegocio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.CpNegocio.Services
{
    public class CategoriaService : ICategoriaService
    {
        private readonly ICategoriaRepositorio _categoriaRepositorio;

        public CategoriaService(ICategoriaRepositorio categoriaRepositorio)
        {
            _categoriaRepositorio = categoriaRepositorio;
        }

        public bool Actualizar(Categorias categorias, out string mensaje)
        {
            mensaje = string.Empty;

            if (categorias.IdCategoria <= 0)
                throw new ArgumentException("La categoria debe tener un Id válido");

            if (string.IsNullOrEmpty(categorias.NombreCategoria) || string.IsNullOrWhiteSpace(categorias.NombreCategoria))
                mensaje = ("El campo nombre de la categoria es requerido.");

            if (string.IsNullOrEmpty(mensaje))
                return _categoriaRepositorio.Actualizar(categorias, out mensaje);
            else
                return false;
        }

        public int Crear(Categorias categorias, out string mensaje)
        {
            mensaje = string.Empty;

            if (string.IsNullOrEmpty(categorias.NombreCategoria) || string.IsNullOrWhiteSpace(categorias.NombreCategoria))
                mensaje = ("El campo nombre de la categoria es requerido.");

            if (string.IsNullOrEmpty(mensaje))
                return _categoriaRepositorio.Insertar(categorias, out mensaje);
            else
                return 0;
        }

        public bool Eliminar(int id, out string mensaje)
        {
            mensaje = string.Empty;
            if (id <= 0)
                throw new ArgumentException("Id invalido para eliminar");
            else
                return _categoriaRepositorio.Eliminar(id, out mensaje);
        }

        public List<Categorias> ListarCategorias()
        {
            return _categoriaRepositorio.Listar();
        }

        public Categorias ObtenerPorId(int id)
        {
            throw new NotImplementedException();
        }
    }
}
