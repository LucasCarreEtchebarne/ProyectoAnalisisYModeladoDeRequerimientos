using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Prototipo.Models
{
    [Table("DETALLE_FACTURA")]
    public class DetalleFactura
    {
        [Key]
        public int IdDetalleFactura { get; set; }

        public int IdFactura { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name = "Concepto")]
        public string TipoConcepto { get; set; }

        public int? IdReserva { get; set; }
        public int? IdPedido { get; set; }
        public int? IdEvento { get; set; }

        [Required]
        [StringLength(250)]
        public string Descripcion { get; set; }

        [Range(0.001, 99999999, ErrorMessage = "La cantidad debe ser mayor que cero.")]
        public decimal Cantidad { get; set; }

        [Range(0, 99999999, ErrorMessage = "El precio unitario no puede ser negativo.")]
        [Display(Name = "Precio unitario")]
        public decimal PrecioUnitario { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public decimal Subtotal { get; set; }

        [ForeignKey("IdFactura")]
        public virtual Factura Factura { get; set; }

        [ForeignKey("IdReserva")]
        public virtual Reserva Reserva { get; set; }

        [ForeignKey("IdPedido")]
        public virtual Pedido Pedido { get; set; }

        [ForeignKey("IdEvento")]
        public virtual Evento Evento { get; set; }
    }
}
