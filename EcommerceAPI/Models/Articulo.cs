using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceAPI.Models
{
    public class Articulo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string CodigoArticulo { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string Nombre { get; set; } = string.Empty;

        public string? Descripcion { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal PrecioUsuario { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal PrecioTaller { get; set; }

        [Required]
        public int CategoriaId { get; set; }

        public int? SubcategoriaId { get; set; }

        public int Stock { get; set; } = 0;

        [StringLength(500)]
        public string? Imagen { get; set; }

        public bool Activo { get; set; } = true;

        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        // Propiedades de navegación
        public virtual Categoria Categoria { get; set; } = null!;
        public virtual Subcategoria? Subcategoria { get; set; }
        public virtual ICollection<ArticuloTaller> ArticuloTalleres { get; set; } = new List<ArticuloTaller>();
    }
}
