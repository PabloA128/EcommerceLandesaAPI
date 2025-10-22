using EcommerceAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EcommerceAPI.Data
{
    public class EcommerceContext : DbContext
    {
        public EcommerceContext(DbContextOptions<EcommerceContext> options) : base(options)
        {
        }

        // DbSets para las entidades
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Subcategoria> Subcategorias { get; set; }
        public DbSet<Articulo> Articulos { get; set; }
        public DbSet<ArticuloTaller> ArticuloTalleres { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración adicional si es necesaria
            modelBuilder.Entity<ArticuloTaller>()
                .HasKey(at => at.Id);

            modelBuilder.Entity<ArticuloTaller>()
                .HasOne(at => at.Articulo)
                .WithMany(a => a.ArticuloTalleres)
                .HasForeignKey(at => at.ArticuloId);

            // Índices para mejorar el rendimiento
            modelBuilder.Entity<Categoria>()
                .HasIndex(c => c.Nombre)
                .IsUnique();

            modelBuilder.Entity<Articulo>()
                .HasIndex(a => a.CodigoArticulo)
                .IsUnique();

            modelBuilder.Entity<Articulo>()
                .HasIndex(a => a.Nombre);

            modelBuilder.Entity<Subcategoria>()
                .HasIndex(s => new { s.Nombre, s.CategoriaId })
                .IsUnique();
        }
    }
}
