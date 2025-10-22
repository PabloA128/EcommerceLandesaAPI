using EcommerceAPI.Data;
using EcommerceAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceAPI.Repositories
{
    public interface ICategoriaRepository
    {
        Task<IEnumerable<Categoria>> GetAllAsync();
        Task<Categoria?> GetByIdAsync(int id);
        Task<Categoria> CreateAsync(Categoria categoria);
        Task<Categoria> UpdateAsync(Categoria categoria);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<IEnumerable<Categoria>> GetActiveAsync();
    }

    public class CategoriaRepository : ICategoriaRepository
    {
        private readonly EcommerceContext _context;

        public CategoriaRepository(EcommerceContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Categoria>> GetAllAsync()
        {
            return await _context.Categorias
                .Include(c => c.Subcategorias)
                .Include(c => c.Articulos)
                .OrderBy(c => c.Nombre)
                .ToListAsync();
        }

        public async Task<Categoria?> GetByIdAsync(int id)
        {
            return await _context.Categorias
                .Include(c => c.Subcategorias)
                .Include(c => c.Articulos)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Categoria> CreateAsync(Categoria categoria)
        {
            _context.Categorias.Add(categoria);
            await _context.SaveChangesAsync();
            return categoria;
        }

        public async Task<Categoria> UpdateAsync(Categoria categoria)
        {
            _context.Entry(categoria).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return categoria;
        }

        public async Task DeleteAsync(int id)
        {
            var categoria = await _context.Categorias.FindAsync(id);
            if (categoria != null)
            {
                _context.Categorias.Remove(categoria);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Categorias.AnyAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Categoria>> GetActiveAsync()
        {
            return await _context.Categorias
                .Where(c => c.Activa)
                .Include(c => c.Subcategorias.Where(s => s.Activa))
                .Include(c => c.Articulos)
                .OrderBy(c => c.Nombre)
                .ToListAsync();
        }
    }
}
