using System.ComponentModel.DataAnnotations;

namespace EcommerceAPI.DTOs
{
    public class CategoriaDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public bool Activa { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int CantidadSubcategorias { get; set; }
        public int CantidadArticulos { get; set; }
    }

    public class SubcategoriaDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public bool Activa { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int CategoriaId { get; set; }
        public string CategoriaNombre { get; set; } = string.Empty;
        public int CantidadArticulos { get; set; }
    }

    public class ArticuloDTO
    {
        public int Id { get; set; }
        public string CodigoArticulo { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public decimal PrecioUsuario { get; set; }
        public decimal PrecioTaller { get; set; }
        public int Stock { get; set; }
        public string? Imagen { get; set; }
        public bool Activo { get; set; }
        public DateTime FechaCreacion { get; set; }

        // Información de categoría y subcategoría
        public int CategoriaId { get; set; }
        public string CategoriaNombre { get; set; } = string.Empty;
        public int? SubcategoriaId { get; set; }
        public string? SubcategoriaNombre { get; set; }

        // IDs de talleres que manejan este artículo
        public List<int> TallerIds { get; set; } = new List<int>();
    }

    public class ArticuloBusquedaDTO
    {
        public int Id { get; set; }
        public string CodigoArticulo { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public decimal PrecioUsuario { get; set; }
        public decimal PrecioTaller { get; set; }
        public int Stock { get; set; }
        public string? Imagen { get; set; }
        public string CategoriaNombre { get; set; } = string.Empty;
        public string? SubcategoriaNombre { get; set; }
    }

    public class BusquedaRequest
    {
        public string? Termino { get; set; }
        public int? CategoriaId { get; set; }
        public int? SubcategoriaId { get; set; }
        public decimal? PrecioMin { get; set; }
        public decimal? PrecioMax { get; set; }
        public bool SoloConStock { get; set; } = false;
        public int Pagina { get; set; } = 1;
        public int TamanoPagina { get; set; } = 20;
    }
}
