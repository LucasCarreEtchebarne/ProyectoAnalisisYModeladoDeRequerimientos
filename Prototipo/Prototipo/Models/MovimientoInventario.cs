using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Prototipo.Models
{
    [Table("MOVIMIENTO_INVENTARIO")]
    public class MovimientoInventario
    {
        [Key]
        public long IdMovimiento { get; set; }

        [Display(Name = "Producto")]
        public int IdProducto { get; set; }

        [Display(Name = "Registrado por")]
        public int IdUsuario { get; set; }

        [Display(Name = "Pedido")]
        public int? IdPedido { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name = "Tipo de movimiento")]
        public string TipoMovimiento { get; set; }

        [Range(0.001, 99999999, ErrorMessage = "La cantidad debe ser mayor que cero.")]
        public decimal Cantidad { get; set; }

        [Display(Name = "Stock resultante")]
        public decimal StockResultante { get; set; }

        [Display(Name = "Fecha y hora")]
        public DateTime FechaHora { get; set; }

        [StringLength(250)]
        public string Motivo { get; set; }

        [ForeignKey("IdProducto")]
        public virtual Inventario Producto { get; set; }

        [ForeignKey("IdUsuario")]
        public virtual Usuario Usuario { get; set; }

        [ForeignKey("IdPedido")]
        public virtual Pedido Pedido { get; set; }
    }
}
