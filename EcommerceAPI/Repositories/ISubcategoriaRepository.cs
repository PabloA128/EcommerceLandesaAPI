using EcommerceAPI.Data;
using EcommerceAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceAPI.Repositories
{
    public interface ISubcategoriaRepository
    {
        Task<IEnumerable<Subcategoria>> GetAllAsync();
        Task<Subcategoria?> GetByIdAsync(int id);
        Task<IEnumerable<Subcategoria>> GetByCategoriaIdAsync(int categoriaId);
        Task<Subcategoria> CreateAsync(Subcategoria subcategoria);
        Task<Subcategoria> UpdateAsync(Subcategoria subcategoria);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<IEnumerable<Subcategoria>> GetActiveAsync();
        Task<IEnumerable<Subcategoria>> GetActiveByCategoriaAsync(int categoriaId);
    }

    public class SubcategoriaRepository : ISubcategoriaRepository
    {
        private readonly EcommerceContext _context;

        public SubcategoriaRepository(EcommerceContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Subcategoria>> GetAllAsync()
        {
            return await _context.Subcategorias
                .Include(s => s.Categoria)
                .Include(s => s.Articulos)
                .OrderBy(s => s.Categoria.Nombre)
                .ThenBy(s => s.Nombre)
                .ToListAsync();
        }

        public async Task<Subcategoria?> GetByIdAsync(int id)
        {
            return await _context.Subcategorias
                .Include(s => s.Categoria)
                .Include(s => s.Articulos)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<IEnumerable<Subcategoria>> GetByCategoriaIdAsync(int categoriaId)
        {
            return await _context.Subcategorias
                .Where(s => s.CategoriaId == categoriaId)
                .Include(s => s.Categoria)
                .OrderBy(s => s.Nombre)
                .ToListAsync();
        }

        public async Task<Subcategoria> CreateAsync(Subcategoria subcategoria)
        {
            _context.Subcategorias.Add(subcategoria);
            await _context.SaveChangesAsync();
            return subcategoria;
        }

        public async Task<Subcategoria> UpdateAsync(Subcategoria subcategoria)
        {
            _context.Entry(subcategoria).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return subcategoria;
        }

        public async Task DeleteAsync(int id)
        {
            var subcategoria = await _context.Subcategorias.FindAsync(id);
            if (subcategoria != null)
            {
                _context.Subcategorias.Remove(subcategoria);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Subcategorias.AnyAsync(s => s.Id == id);
        }

        public async Task<IEnumerable<Subcategoria>> GetActiveAsync()
        {
            return await _context.Subcategorias
                .Where(s => s.Activa)
                .Include(s => s.Categoria)
                .Include(s => s.Articulos)
                .OrderBy(s => s.Categoria.Nombre)
                .ThenBy(s => s.Nombre)
                .ToListAsync();
        }

        public async Task<IEnumerable<Subcategoria>> GetActiveByCategoriaAsync(int categoriaId)
        {
            return await _context.Subcategorias
                .Where(s => s.Activa && s.CategoriaId == categoriaId)
                .Include(s => s.Categoria)
                .Include(s => s.Articulos)
                .OrderBy(s => s.Nombre)
                .ToListAsync();
        }
    }
}
