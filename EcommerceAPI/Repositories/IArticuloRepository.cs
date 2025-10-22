using EcommerceAPI.Data;
using EcommerceAPI.Models;
using EcommerceAPI.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceAPI.Repositories
{
    public interface IArticuloRepository
    {
        Task<IEnumerable<Articulo>> GetAllAsync();
        Task<Articulo?> GetByIdAsync(int id);
        Task<Articulo?> GetByCodigoAsync(string codigo);
        Task<IEnumerable<Articulo>> GetByCategoriaIdAsync(int categoriaId);
        Task<IEnumerable<Articulo>> GetBySubcategoriaIdAsync(int subcategoriaId);
        Task<IEnumerable<Articulo>> GetActiveAsync();
        Task<IEnumerable<Articulo>> GetActiveByCategoriaAsync(int categoriaId);
        Task<IEnumerable<Articulo>> GetActiveBySubcategoriaAsync(int subcategoriaId);
        Task<Articulo> CreateAsync(Articulo articulo);
        Task<Articulo> UpdateAsync(Articulo articulo);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> ExistsByCodigoAsync(string codigo);
        Task<IEnumerable<Articulo>> BuscarArticulosAsync(BusquedaRequest request);
        Task<int> GetTotalCountAsync(BusquedaRequest request);
    }

    public class ArticuloRepository : IArticuloRepository
    {
        private readonly EcommerceContext _context;

        public ArticuloRepository(EcommerceContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Articulo>> GetAllAsync()
        {
            return await _context.Articulos
                .Include(a => a.Categoria)
                .Include(a => a.Subcategoria)
                .Include(a => a.ArticuloTalleres)
                .OrderBy(a => a.Nombre)
                .ToListAsync();
        }

        public async Task<Articulo?> GetByIdAsync(int id)
        {
            return await _context.Articulos
                .Include(a => a.Categoria)
                .Include(a => a.Subcategoria)
                .Include(a => a.ArticuloTalleres)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Articulo?> GetByCodigoAsync(string codigo)
        {
            return await _context.Articulos
                .Include(a => a.Categoria)
                .Include(a => a.Subcategoria)
                .Include(a => a.ArticuloTalleres)
                .FirstOrDefaultAsync(a => a.CodigoArticulo == codigo);
        }

        public async Task<IEnumerable<Articulo>> GetByCategoriaIdAsync(int categoriaId)
        {
            return await _context.Articulos
                .Where(a => a.CategoriaId == categoriaId)
                .Include(a => a.Categoria)
                .Include(a => a.Subcategoria)
                .Include(a => a.ArticuloTalleres)
                .OrderBy(a => a.Nombre)
                .ToListAsync();
        }

        public async Task<IEnumerable<Articulo>> GetBySubcategoriaIdAsync(int subcategoriaId)
        {
            return await _context.Articulos
                .Where(a => a.SubcategoriaId == subcategoriaId)
                .Include(a => a.Categoria)
                .Include(a => a.Subcategoria)
                .Include(a => a.ArticuloTalleres)
                .OrderBy(a => a.Nombre)
                .ToListAsync();
        }

        public async Task<IEnumerable<Articulo>> GetActiveAsync()
        {
            return await _context.Articulos
                .Where(a => a.Activo)
                .Include(a => a.Categoria)
                .Include(a => a.Subcategoria)
                .Include(a => a.ArticuloTalleres)
                .OrderBy(a => a.Nombre)
                .ToListAsync();
        }

        public async Task<IEnumerable<Articulo>> GetActiveByCategoriaAsync(int categoriaId)
        {
            return await _context.Articulos
                .Where(a => a.Activo && a.CategoriaId == categoriaId)
                .Include(a => a.Categoria)
                .Include(a => a.Subcategoria)
                .Include(a => a.ArticuloTalleres)
                .OrderBy(a => a.Nombre)
                .ToListAsync();
        }

        public async Task<IEnumerable<Articulo>> GetActiveBySubcategoriaAsync(int subcategoriaId)
        {
            return await _context.Articulos
                .Where(a => a.Activo && a.SubcategoriaId == subcategoriaId)
                .Include(a => a.Categoria)
                .Include(a => a.Subcategoria)
                .Include(a => a.ArticuloTalleres)
                .OrderBy(a => a.Nombre)
                .ToListAsync();
        }

        public async Task<Articulo> CreateAsync(Articulo articulo)
        {
            _context.Articulos.Add(articulo);
            await _context.SaveChangesAsync();
            return articulo;
        }

        public async Task<Articulo> UpdateAsync(Articulo articulo)
        {
            _context.Entry(articulo).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return articulo;
        }

        public async Task DeleteAsync(int id)
        {
            var articulo = await _context.Articulos.FindAsync(id);
            if (articulo != null)
            {
                _context.Articulos.Remove(articulo);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Articulos.AnyAsync(a => a.Id == id);
        }

        public async Task<bool> ExistsByCodigoAsync(string codigo)
        {
            return await _context.Articulos.AnyAsync(a => a.CodigoArticulo == codigo);
        }

        public async Task<IEnumerable<Articulo>> BuscarArticulosAsync(BusquedaRequest request)
        {
            var query = _context.Articulos
                .Where(a => a.Activo)
                .Include(a => a.Categoria)
                .Include(a => a.Subcategoria)
                .Include(a => a.ArticuloTalleres)
                .AsQueryable();

            // Filtro por término de búsqueda
            if (!string.IsNullOrEmpty(request.Termino))
            {
                var termino = request.Termino.ToLower();
                query = query.Where(a =>
                    a.Nombre.ToLower().Contains(termino) ||
                    a.Descripcion.ToLower().Contains(termino) ||
                    a.CodigoArticulo.ToLower().Contains(termino)
                );
            }

            // Filtro por categoría
            if (request.CategoriaId.HasValue)
            {
                query = query.Where(a => a.CategoriaId == request.CategoriaId.Value);
            }

            // Filtro por subcategoría
            if (request.SubcategoriaId.HasValue)
            {
                query = query.Where(a => a.SubcategoriaId == request.SubcategoriaId.Value);
            }

            // Filtro por precio mínimo
            if (request.PrecioMin.HasValue)
            {
                query = query.Where(a => a.PrecioUsuario >= request.PrecioMin.Value);
            }

            // Filtro por precio máximo
            if (request.PrecioMax.HasValue)
            {
                query = query.Where(a => a.PrecioUsuario <= request.PrecioMax.Value);
            }

            // Filtro por stock
            if (request.SoloConStock)
            {
                query = query.Where(a => a.Stock > 0);
            }

            // Aplicar paginación
            var skip = (request.Pagina - 1) * request.TamanoPagina;
            query = query.Skip(skip).Take(request.TamanoPagina);

            return await query.OrderBy(a => a.Nombre).ToListAsync();
        }

        public async Task<int> GetTotalCountAsync(BusquedaRequest request)
        {
            var query = _context.Articulos
                .Where(a => a.Activo)
                .AsQueryable();

            // Filtro por término de búsqueda
            if (!string.IsNullOrEmpty(request.Termino))
            {
                var termino = request.Termino.ToLower();
                query = query.Where(a =>
                    a.Nombre.ToLower().Contains(termino) ||
                    a.Descripcion.ToLower().Contains(termino) ||
                    a.CodigoArticulo.ToLower().Contains(termino)
                );
            }

            // Filtro por categoría
            if (request.CategoriaId.HasValue)
            {
                query = query.Where(a => a.CategoriaId == request.CategoriaId.Value);
            }

            // Filtro por subcategoría
            if (request.SubcategoriaId.HasValue)
            {
                query = query.Where(a => a.SubcategoriaId == request.SubcategoriaId.Value);
            }

            // Filtro por precio mínimo
            if (request.PrecioMin.HasValue)
            {
                query = query.Where(a => a.PrecioUsuario >= request.PrecioMin.Value);
            }

            // Filtro por precio máximo
            if (request.PrecioMax.HasValue)
            {
                query = query.Where(a => a.PrecioUsuario <= request.PrecioMax.Value);
            }

            // Filtro por stock
            if (request.SoloConStock)
            {
                query = query.Where(a => a.Stock > 0);
            }

            return await query.CountAsync();
        }
    }
}
