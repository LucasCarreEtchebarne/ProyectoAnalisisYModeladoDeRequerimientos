using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Prototipo.Models
{
    [Table("DETALLE_PEDIDO")]
    public class DetallePedido
    {
        [Key]
        public int IdDetallePedido { get; set; }

        public int IdPedido { get; set; }

        [Display(Name = "Producto")]
        public int IdProductoMenu { get; set; }

        [Range(1, 999, ErrorMessage = "La cantidad debe estar entre 1 y 999.")]
        public int Cantidad { get; set; }

        [Display(Name = "Precio unitario")]
        public decimal PrecioUnitario { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public decimal Subtotal { get; set; }

        [ForeignKey("IdPedido")]
        public virtual Pedido Pedido { get; set; }

        [ForeignKey("IdProductoMenu")]
        public virtual ProductoMenu ProductoMenu { get; set; }
    }
}
