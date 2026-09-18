using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Prototipo.Models
{
    [Table("MENU")]
    public class ProductoMenu
    {
        [Key]
        public int IdProductoMenu { get; set; }

        [Required(ErrorMessage = "El nombre del producto es obligatorio.")]
        [StringLength(100)]
        [Display(Name = "Producto")]
        public string NombreProducto { get; set; }

        [Required(ErrorMessage = "La categoria es obligatoria.")]
        [StringLength(50)]
        [Display(Name = "Categoria")]
        public string CategoriaMenu { get; set; }

        [Range(0, 99999999, ErrorMessage = "El precio no puede ser negativo.")]
        public decimal Precio { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name = "Estado")]
        public string EstadoProductoMenu { get; set; }

        [StringLength(250)]
        public string Descripcion { get; set; }

        public virtual ICollection<RecetaProducto> Receta { get; set; }
        public virtual ICollection<DetallePedido> DetallesPedido { get; set; }
    }
}
