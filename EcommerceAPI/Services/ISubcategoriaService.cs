using EcommerceAPI.DTOs;
using EcommerceAPI.Models;
using EcommerceAPI.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceAPI.Services
{
    public interface ISubcategoriaService
    {
        Task<IEnumerable<SubcategoriaDTO>> GetAllAsync();
        Task<SubcategoriaDTO?> GetByIdAsync(int id);
        Task<IEnumerable<SubcategoriaDTO>> GetByCategoriaIdAsync(int categoriaId);
        Task<SubcategoriaDTO> CreateAsync(SubcategoriaDTO subcategoriaDto);
        Task<SubcategoriaDTO> UpdateAsync(int id, SubcategoriaDTO subcategoriaDto);
        Task DeleteAsync(int id);
        Task<IEnumerable<SubcategoriaDTO>> GetActiveAsync();
        Task<IEnumerable<SubcategoriaDTO>> GetActiveByCategoriaAsync(int categoriaId);
    }

    public class SubcategoriaService : ISubcategoriaService
    {
        private readonly ISubcategoriaRepository _subcategoriaRepository;

        public SubcategoriaService(ISubcategoriaRepository subcategoriaRepository)
        {
            _subcategoriaRepository = subcategoriaRepository;
        }

        public async Task<IEnumerable<SubcategoriaDTO>> GetAllAsync()
        {
            var subcategorias = await _subcategoriaRepository.GetAllAsync();
            return subcategorias.Select(MapToDTO);
        }

        public async Task<SubcategoriaDTO?> GetByIdAsync(int id)
        {
            var subcategoria = await _subcategoriaRepository.GetByIdAsync(id);
            return subcategoria != null ? MapToDTO(subcategoria) : null;
        }

        public async Task<IEnumerable<SubcategoriaDTO>> GetByCategoriaIdAsync(int categoriaId)
        {
            var subcategorias = await _subcategoriaRepository.GetByCategoriaIdAsync(categoriaId);
            return subcategorias.Select(MapToDTO);
        }

        public async Task<SubcategoriaDTO> CreateAsync(SubcategoriaDTO subcategoriaDto)
        {
            var subcategoria = new Subcategoria
            {
                Nombre = subcategoriaDto.Nombre,
                Descripcion = subcategoriaDto.Descripcion,
                CategoriaId = subcategoriaDto.CategoriaId,
                Activa = subcategoriaDto.Activa
            };

            subcategoria = await _subcategoriaRepository.CreateAsync(subcategoria);
            return MapToDTO(subcategoria);
        }

        public async Task<SubcategoriaDTO> UpdateAsync(int id, SubcategoriaDTO subcategoriaDto)
        {
            var subcategoria = await _subcategoriaRepository.GetByIdAsync(id);
            if (subcategoria == null)
            {
                throw new KeyNotFoundException($"Subcategoría con ID {id} no encontrada");
            }

            subcategoria.Nombre = subcategoriaDto.Nombre;
            subcategoria.Descripcion = subcategoriaDto.Descripcion;
            subcategoria.CategoriaId = subcategoriaDto.CategoriaId;
            subcategoria.Activa = subcategoriaDto.Activa;

            subcategoria = await _subcategoriaRepository.UpdateAsync(subcategoria);
            return MapToDTO(subcategoria);
        }

        public async Task DeleteAsync(int id)
        {
            if (!await _subcategoriaRepository.ExistsAsync(id))
            {
                throw new KeyNotFoundException($"Subcategoría con ID {id} no encontrada");
            }

            await _subcategoriaRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<SubcategoriaDTO>> GetActiveAsync()
        {
            var subcategorias = await _subcategoriaRepository.GetActiveAsync();
            return subcategorias.Select(MapToDTO);
        }

        public async Task<IEnumerable<SubcategoriaDTO>> GetActiveByCategoriaAsync(int categoriaId)
        {
            var subcategorias = await _subcategoriaRepository.GetActiveByCategoriaAsync(categoriaId);
            return subcategorias.Select(MapToDTO);
        }

        private static SubcategoriaDTO MapToDTO(Subcategoria subcategoria)
        {
            return new SubcategoriaDTO
            {
                Id = subcategoria.Id,
                Nombre = subcategoria.Nombre,
                Descripcion = subcategoria.Descripcion,
                Activa = subcategoria.Activa,
                FechaCreacion = subcategoria.FechaCreacion,
                CategoriaId = subcategoria.CategoriaId,
                CategoriaNombre = subcategoria.Categoria?.Nombre ?? string.Empty,
                CantidadArticulos = subcategoria.Articulos?.Count ?? 0
            };
        }
    }
}
