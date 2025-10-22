using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceAPI.Models
{
    public class ArticuloTaller
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int ArticuloId { get; set; }

        [Required]
        public int TallerId { get; set; }

        // Propiedades de navegación
        public virtual Articulo Articulo { get; set; } = null!;
    }
}
