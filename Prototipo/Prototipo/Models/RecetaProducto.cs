using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Prototipo.Models
{
    [Table("RECETA_PRODUCTO")]
    public class RecetaProducto
    {
        [Key]
        public int IdRecetaProducto { get; set; }

        [Display(Name = "Producto del menu")]
        public int IdProductoMenu { get; set; }

        [Display(Name = "Insumo")]
        public int IdProducto { get; set; }

        [Range(0.001, 99999, ErrorMessage = "La cantidad utilizada debe ser mayor que cero.")]
        [Display(Name = "Cantidad utilizada")]
        public decimal CantidadUtilizada { get; set; }

        [ForeignKey("IdProductoMenu")]
        public virtual ProductoMenu ProductoMenu { get; set; }

        [ForeignKey("IdProducto")]
        public virtual Inventario Insumo { get; set; }
    }
}
