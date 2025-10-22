using EcommerceAPI.DTOs;
using EcommerceAPI.Models;
using EcommerceAPI.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceAPI.Services
{
    public interface ICategoriaService
    {
        Task<IEnumerable<CategoriaDTO>> GetAllAsync();
        Task<CategoriaDTO?> GetByIdAsync(int id);
        Task<CategoriaDTO> CreateAsync(CategoriaDTO categoriaDto);
        Task<CategoriaDTO> UpdateAsync(int id, CategoriaDTO categoriaDto);
        Task DeleteAsync(int id);
        Task<IEnumerable<CategoriaDTO>> GetActiveAsync();
    }

    public class CategoriaService : ICategoriaService
    {
        private readonly ICategoriaRepository _categoriaRepository;

        public CategoriaService(ICategoriaRepository categoriaRepository)
        {
            _categoriaRepository = categoriaRepository;
        }

        public async Task<IEnumerable<CategoriaDTO>> GetAllAsync()
        {
            var categorias = await _categoriaRepository.GetAllAsync();
            return categorias.Select(MapToDTO);
        }

        public async Task<CategoriaDTO?> GetByIdAsync(int id)
        {
            var categoria = await _categoriaRepository.GetByIdAsync(id);
            return categoria != null ? MapToDTO(categoria) : null;
        }

        public async Task<CategoriaDTO> CreateAsync(CategoriaDTO categoriaDto)
        {
            var categoria = new Categoria
            {
                Nombre = categoriaDto.Nombre,
                Descripcion = categoriaDto.Descripcion,
                Activa = categoriaDto.Activa
            };

            categoria = await _categoriaRepository.CreateAsync(categoria);
            return MapToDTO(categoria);
        }

        public async Task<CategoriaDTO> UpdateAsync(int id, CategoriaDTO categoriaDto)
        {
            var categoria = await _categoriaRepository.GetByIdAsync(id);
            if (categoria == null)
            {
                throw new KeyNotFoundException($"Categoría con ID {id} no encontrada");
            }

            categoria.Nombre = categoriaDto.Nombre;
            categoria.Descripcion = categoriaDto.Descripcion;
            categoria.Activa = categoriaDto.Activa;

            categoria = await _categoriaRepository.UpdateAsync(categoria);
            return MapToDTO(categoria);
        }

        public async Task DeleteAsync(int id)
        {
            if (!await _categoriaRepository.ExistsAsync(id))
            {
                throw new KeyNotFoundException($"Categoría con ID {id} no encontrada");
            }

            await _categoriaRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<CategoriaDTO>> GetActiveAsync()
        {
            var categorias = await _categoriaRepository.GetActiveAsync();
            return categorias.Select(MapToDTO);
        }

        private static CategoriaDTO MapToDTO(Categoria categoria)
        {
            return new CategoriaDTO
            {
                Id = categoria.Id,
                Nombre = categoria.Nombre,
                Descripcion = categoria.Descripcion,
                Activa = categoria.Activa,
                FechaCreacion = categoria.FechaCreacion,
                CantidadSubcategorias = categoria.Subcategorias?.Count ?? 0,
                CantidadArticulos = categoria.Articulos?.Count ?? 0
            };
        }
    }
}
