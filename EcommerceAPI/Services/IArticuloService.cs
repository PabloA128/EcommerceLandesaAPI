using EcommerceAPI.DTOs;
using EcommerceAPI.Models;
using EcommerceAPI.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceAPI.Services
{
    public interface IArticuloService
    {
        Task<IEnumerable<ArticuloDTO>> GetAllAsync();
        Task<ArticuloDTO?> GetByIdAsync(int id);
        Task<ArticuloDTO?> GetByCodigoAsync(string codigo);
        Task<IEnumerable<ArticuloDTO>> GetByCategoriaIdAsync(int categoriaId);
        Task<IEnumerable<ArticuloDTO>> GetBySubcategoriaIdAsync(int subcategoriaId);
        Task<IEnumerable<ArticuloDTO>> GetActiveAsync();
        Task<IEnumerable<ArticuloDTO>> GetActiveByCategoriaAsync(int categoriaId);
        Task<IEnumerable<ArticuloDTO>> GetActiveBySubcategoriaAsync(int subcategoriaId);
        Task<ArticuloDTO> CreateAsync(ArticuloDTO articuloDto);
        Task<ArticuloDTO> UpdateAsync(int id, ArticuloDTO articuloDto);
        Task DeleteAsync(int id);
        Task<IEnumerable<ArticuloBusquedaDTO>> BuscarArticulosAsync(BusquedaRequest request);
        Task<int> GetTotalCountAsync(BusquedaRequest request);
    }

    public class ArticuloService : IArticuloService
    {
        private readonly IArticuloRepository _articuloRepository;

        public ArticuloService(IArticuloRepository articuloRepository)
        {
            _articuloRepository = articuloRepository;
        }

        public async Task<IEnumerable<ArticuloDTO>> GetAllAsync()
        {
            var articulos = await _articuloRepository.GetAllAsync();
            return articulos.Select(MapToDTO);
        }

        public async Task<ArticuloDTO?> GetByIdAsync(int id)
        {
            var articulo = await _articuloRepository.GetByIdAsync(id);
            return articulo != null ? MapToDTO(articulo) : null;
        }

        public async Task<ArticuloDTO?> GetByCodigoAsync(string codigo)
        {
            var articulo = await _articuloRepository.GetByCodigoAsync(codigo);
            return articulo != null ? MapToDTO(articulo) : null;
        }

        public async Task<IEnumerable<ArticuloDTO>> GetByCategoriaIdAsync(int categoriaId)
        {
            var articulos = await _articuloRepository.GetByCategoriaIdAsync(categoriaId);
            return articulos.Select(MapToDTO);
        }

        public async Task<IEnumerable<ArticuloDTO>> GetBySubcategoriaIdAsync(int subcategoriaId)
        {
            var articulos = await _articuloRepository.GetBySubcategoriaIdAsync(subcategoriaId);
            return articulos.Select(MapToDTO);
        }

        public async Task<IEnumerable<ArticuloDTO>> GetActiveAsync()
        {
            var articulos = await _articuloRepository.GetActiveAsync();
            return articulos.Select(MapToDTO);
        }

        public async Task<IEnumerable<ArticuloDTO>> GetActiveByCategoriaAsync(int categoriaId)
        {
            var articulos = await _articuloRepository.GetActiveByCategoriaAsync(categoriaId);
            return articulos.Select(MapToDTO);
        }

        public async Task<IEnumerable<ArticuloDTO>> GetActiveBySubcategoriaAsync(int subcategoriaId)
        {
            var articulos = await _articuloRepository.GetActiveBySubcategoriaAsync(subcategoriaId);
            return articulos.Select(MapToDTO);
        }

        public async Task<ArticuloDTO> CreateAsync(ArticuloDTO articuloDto)
        {
            var articulo = new Articulo
            {
                CodigoArticulo = articuloDto.CodigoArticulo,
                Nombre = articuloDto.Nombre,
                Descripcion = articuloDto.Descripcion,
                PrecioUsuario = articuloDto.PrecioUsuario,
                PrecioTaller = articuloDto.PrecioTaller,
                CategoriaId = articuloDto.CategoriaId,
                SubcategoriaId = articuloDto.SubcategoriaId,
                Stock = articuloDto.Stock,
                Imagen = articuloDto.Imagen,
                Activo = articuloDto.Activo
            };

            articulo = await _articuloRepository.CreateAsync(articulo);
            return MapToDTO(articulo);
        }

        public async Task<ArticuloDTO> UpdateAsync(int id, ArticuloDTO articuloDto)
        {
            var articulo = await _articuloRepository.GetByIdAsync(id);
            if (articulo == null)
            {
                throw new KeyNotFoundException($"Artículo con ID {id} no encontrado");
            }

            articulo.CodigoArticulo = articuloDto.CodigoArticulo;
            articulo.Nombre = articuloDto.Nombre;
            articulo.Descripcion = articuloDto.Descripcion;
            articulo.PrecioUsuario = articuloDto.PrecioUsuario;
            articulo.PrecioTaller = articuloDto.PrecioTaller;
            articulo.CategoriaId = articuloDto.CategoriaId;
            articulo.SubcategoriaId = articuloDto.SubcategoriaId;
            articulo.Stock = articuloDto.Stock;
            articulo.Imagen = articuloDto.Imagen;
            articulo.Activo = articuloDto.Activo;

            articulo = await _articuloRepository.UpdateAsync(articulo);
            return MapToDTO(articulo);
        }

        public async Task DeleteAsync(int id)
        {
            if (!await _articuloRepository.ExistsAsync(id))
            {
                throw new KeyNotFoundException($"Artículo con ID {id} no encontrado");
            }

            await _articuloRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<ArticuloBusquedaDTO>> BuscarArticulosAsync(BusquedaRequest request)
        {
            var articulos = await _articuloRepository.BuscarArticulosAsync(request);
            return articulos.Select(MapToBusquedaDTO);
        }

        public async Task<int> GetTotalCountAsync(BusquedaRequest request)
        {
            return await _articuloRepository.GetTotalCountAsync(request);
        }

        private static ArticuloDTO MapToDTO(Articulo articulo)
        {
            return new ArticuloDTO
            {
                Id = articulo.Id,
                CodigoArticulo = articulo.CodigoArticulo,
                Nombre = articulo.Nombre,
                Descripcion = articulo.Descripcion,
                PrecioUsuario = articulo.PrecioUsuario,
                PrecioTaller = articulo.PrecioTaller,
                Stock = articulo.Stock,
                Imagen = articulo.Imagen,
                Activo = articulo.Activo,
                FechaCreacion = articulo.FechaCreacion,
                CategoriaId = articulo.CategoriaId,
                CategoriaNombre = articulo.Categoria?.Nombre ?? string.Empty,
                SubcategoriaId = articulo.SubcategoriaId,
                SubcategoriaNombre = articulo.Subcategoria?.Nombre,
                TallerIds = articulo.ArticuloTalleres?.Select(at => at.TallerId).ToList() ?? new List<int>()
            };
        }

        private static ArticuloBusquedaDTO MapToBusquedaDTO(Articulo articulo)
        {
            return new ArticuloBusquedaDTO
            {
                Id = articulo.Id,
                CodigoArticulo = articulo.CodigoArticulo,
                Nombre = articulo.Nombre,
                Descripcion = articulo.Descripcion,
                PrecioUsuario = articulo.PrecioUsuario,
                PrecioTaller = articulo.PrecioTaller,
                Stock = articulo.Stock,
                Imagen = articulo.Imagen,
                CategoriaNombre = articulo.Categoria?.Nombre ?? string.Empty,
                SubcategoriaNombre = articulo.Subcategoria?.Nombre
            };
        }
    }
}
